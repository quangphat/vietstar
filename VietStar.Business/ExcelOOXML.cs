using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace VietStar.Business
{
    public class ExcelOOXML
    {
        public ZipArchive ZipArchiveFile { get; set; }

        public ExcelOOXML(ZipArchive zipArchiveFile)
        {
            ZipArchiveFile = zipArchiveFile;
        }
        public void InsertRow(string sheetName, int rowIndex, int rowNumber, bool copyStyle)
        {
            if (rowNumber <= 0)
                return;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.PreserveWhitespace = true;
                ZipArchiveEntry entry = ZipArchiveFile.Entries.Single(x => x.FullName == "xl/worksheets/sheet" + GetSheetId(sheetName) + ".xml");
                if (entry == null)
                    return;
                using (Stream entryStream = entry.Open())
                {
                    using (StreamReader reader = new StreamReader(entryStream))
                    {
                        string entryData = "";
                        xmlDocument.LoadXml(reader.ReadToEnd());
                        XmlNodeList lstRowNode = xmlDocument.GetElementsByTagName("row");
                        XmlElement rowIndexNode = null;
                        if (lstRowNode != null && lstRowNode.Count > 0)
                        {
                            foreach (XmlElement rowNode in lstRowNode)
                            {
                                if (Convert.ToInt32(rowNode.GetAttribute("r")) == rowIndex)
                                    rowIndexNode = rowNode;
                                else if (Convert.ToInt32(rowNode.GetAttribute("r")) > rowIndex)
                                {
                                    rowNode.Attributes["r"].Value = (Convert.ToInt32(rowNode.GetAttribute("r")) + rowNumber).ToString();
                                    XmlNodeList lstCNode = rowNode.GetElementsByTagName("c");
                                    if (lstCNode != null && lstCNode.Count > 0)
                                    {
                                        foreach (XmlElement cNode in lstCNode)
                                        {
                                            string rAttribute = GetColumnAddress(cNode.GetAttribute("r")) + rowNode.Attributes["r"].Value;
                                            cNode.Attributes["r"].Value = rAttribute;
                                        }
                                    }
                                }
                            }
                        }
                        if (copyStyle == true)
                        {
                            List<XmlElement> lstRowNodeTemp = lstRowNode.Cast<XmlElement>().ToList();
                            // Add new <row> element with style = rowIndex style
                            for (int i = 0; i < rowNumber; i++)
                            {
                                XmlElement newRow = (XmlElement)rowIndexNode.Clone();
                                newRow.Attributes["r"].Value = (rowIndex + i + 1).ToString();
                                XmlNodeList lstCNode = newRow.GetElementsByTagName("c");
                                if (lstCNode != null && lstCNode.Count > 0)
                                {
                                    foreach (XmlElement cNode in lstCNode)
                                    {
                                        string rAttribute = GetColumnAddress(cNode.GetAttribute("r")) + newRow.Attributes["r"].Value;
                                        cNode.Attributes["r"].Value = rAttribute;
                                        cNode.RemoveAttribute("t");
                                        cNode.InnerXml = "";
                                    }
                                }
                                int indexInsert = 0;
                                foreach (XmlElement rowNode in lstRowNodeTemp)
                                {
                                    if (Convert.ToInt32(rowNode.GetAttribute("r")) > (rowIndex + i + 1))
                                    {
                                        lstRowNodeTemp.Insert(indexInsert, newRow);
                                        break;
                                    }
                                    else
                                    {
                                        indexInsert += 1;
                                        if (indexInsert == lstRowNodeTemp.Count)
                                        {
                                            lstRowNodeTemp.Add(newRow);
                                            break;
                                        }
                                    }
                                }
                            }
                            XmlNodeList lstSheetDataNode = xmlDocument.GetElementsByTagName("sheetData");
                            lstSheetDataNode[0].InnerXml = "";
                            foreach (XmlElement rowNode in lstRowNodeTemp)
                            {
                                lstSheetDataNode[0].InnerXml += rowNode.OuterXml;
                            }
                        }

                        // Update old merge cell
                        XmlNodeList lstMergeCellNode = xmlDocument.GetElementsByTagName("mergeCell");
                        if (lstMergeCellNode != null && lstMergeCellNode.Count > 0)
                        {
                            foreach (XmlElement mergeCellNode in lstMergeCellNode)
                            {
                                string[] mergeAddressArray = mergeCellNode.GetAttribute("ref").Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                int startRowIndex = GetRowAddress(mergeAddressArray[0]);
                                int endRowIndex = GetRowAddress(mergeAddressArray[1]);
                                if (rowIndex < startRowIndex)
                                {
                                    string mergeNewAddress = "";
                                    mergeNewAddress = GetColumnAddress(mergeAddressArray[0]) + (startRowIndex + rowNumber).ToString();
                                    mergeNewAddress += ":";
                                    mergeNewAddress += GetColumnAddress(mergeAddressArray[1]) + (endRowIndex + rowNumber).ToString();
                                    mergeCellNode.Attributes["ref"].Value = mergeNewAddress;
                                }
                            }
                        }
                        entryData = xmlDocument.OuterXml;
                        entryStream.SetLength(0);
                        using (StreamWriter writer = new StreamWriter(entryStream))
                        {
                            writer.Write(entryData);
                            writer.Flush();
                        }
                    }
                }
                // Update table address
                List<ZipArchiveEntry> lstTableEntry = ZipArchiveFile.Entries.ToList().FindAll(x => x.FullName.StartsWith("xl/tables/table") && x.FullName.EndsWith(".xml"));
                if (lstTableEntry != null && lstTableEntry.Count > 0)
                {
                    foreach (ZipArchiveEntry tableEntry in lstTableEntry)
                    {
                        using (Stream entryStream = tableEntry.Open())
                        {
                            using (StreamReader reader = new StreamReader(entryStream))
                            {
                                string entryData = "";
                                xmlDocument.LoadXml(reader.ReadToEnd());
                                // <Table> element
                                XmlElement tableNode = (XmlElement)xmlDocument.GetElementsByTagName("table")[0];

                                string tableAddress = tableNode.GetAttribute("ref");
                                string[] addressArray = tableAddress.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                int startRowIndex = GetRowAddress(addressArray[0]);
                                int endRowIndex = GetRowAddress(addressArray[1]);
                                if (rowIndex >= startRowIndex && rowIndex < endRowIndex)
                                {
                                    tableAddress = GetColumnAddress(addressArray[0]) + (GetRowAddress(addressArray[0]) + rowNumber).ToString();
                                    tableAddress += ":";
                                    tableAddress += GetColumnAddress(addressArray[1]) + (GetRowAddress(addressArray[1]) + rowNumber).ToString();
                                    tableNode.Attributes["ref"].Value = tableAddress;

                                    // <autoFilter> element
                                    XmlElement autoFilterNode = (XmlElement)xmlDocument.GetElementsByTagName("autoFilter")[0];
                                    autoFilterNode.Attributes["ref"].Value = tableAddress;

                                    entryData = xmlDocument.OuterXml;
                                    entryStream.SetLength(0);
                                    using (StreamWriter writer = new StreamWriter(entryStream))
                                    {
                                        writer.Write(entryData);
                                        writer.Flush();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetCellData(string sheetName, string address, string data)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.PreserveWhitespace = true;
                ZipArchiveEntry entry = ZipArchiveFile.Entries.Single(x => x.FullName == "xl/worksheets/sheet" + GetSheetId(sheetName) + ".xml");
                if (entry == null)
                    return;
                using (Stream entryStream = entry.Open())
                {
                    using (StreamReader reader = new StreamReader(entryStream))
                    {
                        string entryData = "";
                        xmlDocument.LoadXml(reader.ReadToEnd());
                        XmlNodeList lstNode = xmlDocument.GetElementsByTagName("c");
                        bool existCNodeFlag = false;
                        if (lstNode != null && lstNode.Count > 0)
                        {
                            foreach (XmlElement node in lstNode)
                            {
                                if (node.Attributes["r"].Value == address)
                                {
                                    existCNodeFlag = true;
                                    // Create new <is> element
                                    XmlNode isNode = xmlDocument.CreateNode(XmlNodeType.Element, "is", null);
                                    XmlNode tNode = xmlDocument.CreateNode(XmlNodeType.Element, "t", null);
                                    tNode.InnerText = data;
                                    isNode.AppendChild(tNode);

                                    if (node.Attributes["t"] == null)
                                    {
                                        XmlAttribute tAttribute = xmlDocument.CreateAttribute("t");
                                        node.Attributes.Append(tAttribute);
                                    }
                                    node.Attributes["t"].Value = "inlineStr";
                                    node.InnerXml = isNode.OuterXml;
                                }
                            }
                        }
                        if (existCNodeFlag == false)
                        {
                            // Check exist <row> element
                            bool existRowNodeFlag = false;
                            XmlNodeList lstRowNode = xmlDocument.GetElementsByTagName("row");
                            if (lstRowNode != null && lstRowNode.Count > 0)
                            {
                                foreach (XmlElement rowNode in lstRowNode)
                                {
                                    if (rowNode.Attributes["r"].Value == GetRowAddress(address).ToString())
                                    {
                                        existRowNodeFlag = true;
                                        // Create <c> element
                                        XmlNode cNode = xmlDocument.CreateNode(XmlNodeType.Element, "c", null);
                                        XmlAttribute rAttributeC = xmlDocument.CreateAttribute("r");
                                        rAttributeC.Value = address;
                                        cNode.Attributes.Append(rAttributeC);
                                        XmlAttribute tAttributeC = xmlDocument.CreateAttribute("t");
                                        tAttributeC.Value = "inlineStr";
                                        cNode.Attributes.Append(tAttributeC);
                                        XmlNode isNode = xmlDocument.CreateNode(XmlNodeType.Element, "is", null);
                                        cNode.AppendChild(isNode);
                                        XmlNode tNode = xmlDocument.CreateNode(XmlNodeType.Element, "t", null);
                                        tNode.InnerText = data;
                                        isNode.AppendChild(tNode);

                                        //rowNode.AppendChild(cNode);
                                        List<XmlElement> lstCurCNode = rowNode.ChildNodes.Cast<XmlElement>().ToList();
                                        if (lstCurCNode.Count > 0)
                                        {
                                            lstCurCNode.Add((XmlElement)cNode);
                                            //lstCurCNode.OrderBy(x => GetColumnAddress(x.GetAttribute("r")).Length).ThenBy(x => GetColumnAddress(x.GetAttribute("r")).Reverse());
                                            lstCurCNode = lstCurCNode.OrderBy(x => GetColumnAddress(x.GetAttribute("r")).Length).ThenBy(x => GetColumnAddress(x.GetAttribute("r")).Reverse().First()).ToList();
                                            rowNode.InnerXml = "";
                                            for (int i = 0; i < lstCurCNode.Count; i++)
                                            {
                                                rowNode.InnerXml += lstCurCNode[i].OuterXml;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            if (existRowNodeFlag == false)
                            {
                                // Add <row> element
                                XmlNodeList lstSheetDataNode = xmlDocument.GetElementsByTagName("sheetData");
                                if (lstSheetDataNode != null && lstSheetDataNode.Count > 0)
                                {
                                    int rowIndex = GetRowAddress(address);
                                    // Create new <row> element
                                    XmlNode rowNode = xmlDocument.CreateNode(XmlNodeType.Element, "row", null);
                                    XmlAttribute rAttributeRow = xmlDocument.CreateAttribute("r");
                                    rAttributeRow.Value = rowIndex.ToString();
                                    rowNode.Attributes.Append(rAttributeRow);

                                    XmlNode cNode = xmlDocument.CreateNode(XmlNodeType.Element, "c", null);
                                    XmlAttribute rAttributeC = xmlDocument.CreateAttribute("r");
                                    rAttributeC.Value = address;
                                    cNode.Attributes.Append(rAttributeC);
                                    XmlAttribute tAttributeC = xmlDocument.CreateAttribute("t");
                                    tAttributeC.Value = "inlineStr";
                                    cNode.Attributes.Append(tAttributeC);
                                    rowNode.AppendChild(cNode);

                                    XmlNode isNode = xmlDocument.CreateNode(XmlNodeType.Element, "is", null);
                                    cNode.AppendChild(isNode);
                                    XmlNode tNode = xmlDocument.CreateNode(XmlNodeType.Element, "t", null);
                                    tNode.InnerText = data;
                                    isNode.AppendChild(tNode);
                                    lstSheetDataNode[0].InnerXml += rowNode.OuterXml;
                                }
                            }
                        }
                        entryData = xmlDocument.OuterXml;
                        entryStream.SetLength(0);
                        using (StreamWriter writer = new StreamWriter(entryStream))
                        {
                            writer.Write(entryData);
                            writer.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MergeCell(string sheetName, string address)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.PreserveWhitespace = true;
                ZipArchiveEntry entry = ZipArchiveFile.Entries.Single(x => x.FullName == "xl/worksheets/sheet" + GetSheetId(sheetName) + ".xml");
                if (entry == null)
                    return;
                using (Stream entryStream = entry.Open())
                {
                    using (StreamReader reader = new StreamReader(entryStream))
                    {
                        string entryData = "";
                        xmlDocument.LoadXml(reader.ReadToEnd());
                        // Check confict
                        XmlNodeList lstMergeCellsNode = xmlDocument.GetElementsByTagName("mergeCells");
                        XmlElement mergeCellsNode = null;
                        if (lstMergeCellsNode != null && lstMergeCellsNode.Count > 0)
                        {
                            mergeCellsNode = (XmlElement)lstMergeCellsNode[0];
                            mergeCellsNode.Attributes["count"].Value = (Convert.ToInt32(mergeCellsNode.Attributes["count"].Value) + 1).ToString();
                        }
                        else
                        {
                            XmlNode mergeCellsNodeNew = xmlDocument.CreateNode(XmlNodeType.Element, "mergeCells", null);
                            XmlAttribute countAttribute = xmlDocument.CreateAttribute("count");
                            countAttribute.Value = "1";
                            mergeCellsNodeNew.Attributes.Append(countAttribute);
                            XmlNodeList lstWorkSheetNode = xmlDocument.GetElementsByTagName("workSheet");
                            if (lstWorkSheetNode != null && lstWorkSheetNode.Count > 0)
                                lstWorkSheetNode[0].AppendChild(mergeCellsNodeNew);
                        }
                        XmlNode mergeCellNode = xmlDocument.CreateNode(XmlNodeType.Element, "mergeCell", null);
                        XmlAttribute refAttribute = xmlDocument.CreateAttribute("ref");
                        refAttribute.Value = address;
                        mergeCellNode.Attributes.Append(refAttribute);
                        mergeCellsNode.InnerXml += mergeCellNode.OuterXml;

                        entryData = xmlDocument.OuterXml;
                        entryStream.SetLength(0);
                        using (StreamWriter writer = new StreamWriter(entryStream))
                        {
                            writer.Write(entryData);
                            writer.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SetFormula(string sheetName, string address)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            ZipArchiveEntry entry = ZipArchiveFile.Entries.Single(x => x.FullName == "xl/worksheets/sheet" + GetSheetId(sheetName) + ".xml");
            if (entry == null)
                return false;
            using (Stream entryStream = entry.Open())
            {
                using (StreamReader reader = new StreamReader(entryStream))
                {
                    string entryData = "";
                    xmlDocument.LoadXml(reader.ReadToEnd());
                    XmlNodeList lstNode = xmlDocument.GetElementsByTagName("c");
                    bool existCNodeFlag = false;
                    if (lstNode != null && lstNode.Count > 0)
                    {
                        foreach (XmlElement node in lstNode)
                        {
                            if (node.Attributes["r"].Value == address)
                            {
                                existCNodeFlag = true;
                                // Create new <is> element
                                XmlNode isNode = xmlDocument.CreateNode(XmlNodeType.Element, "is", null);
                                XmlNode tNode = xmlDocument.CreateNode(XmlNodeType.Element, "t", null);
                                tNode.InnerText = "";
                                isNode.AppendChild(tNode);

                                node.Attributes["t"].Value = "inlineStr";
                                node.InnerXml = isNode.OuterXml;
                            }
                        }
                    }
                    if (existCNodeFlag == false)
                    {
                        // Check exist <row> element
                        bool existRowNodeFlag = false;
                        XmlNodeList lstRowNode = xmlDocument.GetElementsByTagName("row");
                        if (lstRowNode != null && lstRowNode.Count > 0)
                        {
                            foreach (XmlElement rowNode in lstRowNode)
                            {
                                if (rowNode.Attributes["r"].Value == GetRowAddress(address).ToString())
                                {
                                    existRowNodeFlag = true;
                                    // Create <c> element
                                    XmlNode cNode = xmlDocument.CreateNode(XmlNodeType.Element, "c", null);
                                    XmlAttribute rAttributeC = xmlDocument.CreateAttribute("r");
                                    rAttributeC.Value = address;
                                    cNode.Attributes.Append(rAttributeC);
                                    XmlAttribute tAttributeC = xmlDocument.CreateAttribute("t");
                                    tAttributeC.Value = "inlineStr";
                                    cNode.Attributes.Append(tAttributeC);
                                    XmlNode isNode = xmlDocument.CreateNode(XmlNodeType.Element, "is", null);
                                    cNode.AppendChild(isNode);
                                    XmlNode tNode = xmlDocument.CreateNode(XmlNodeType.Element, "t", null);
                                    tNode.InnerText = "";
                                    isNode.AppendChild(tNode);

                                    //rowNode.AppendChild(cNode);
                                    List<XmlElement> lstCurCNode = rowNode.ChildNodes.Cast<XmlElement>().ToList();
                                    if (lstCurCNode.Count > 0)
                                    {
                                        lstCurCNode.Add((XmlElement)cNode);
                                        //lstCurCNode.OrderBy(x => GetColumnAddress(x.GetAttribute("r")).Length).ThenBy(x => GetColumnAddress(x.GetAttribute("r")).Reverse());
                                        lstCurCNode = lstCurCNode.OrderBy(x => GetColumnAddress(x.GetAttribute("r")).Length).ThenBy(x => GetColumnAddress(x.GetAttribute("r")).Reverse().First()).ToList();
                                        rowNode.InnerXml = "";
                                        for (int i = 0; i < lstCurCNode.Count; i++)
                                        {
                                            rowNode.InnerXml += lstCurCNode[i].OuterXml;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        if (existRowNodeFlag == false)
                        {
                            // Add <row> element
                            XmlNodeList lstSheetDataNode = xmlDocument.GetElementsByTagName("sheetData");
                            if (lstSheetDataNode != null && lstSheetDataNode.Count > 0)
                            {
                                int rowIndex = GetRowAddress(address);
                                // Create new <row> element
                                XmlNode rowNode = xmlDocument.CreateNode(XmlNodeType.Element, "row", null);
                                XmlAttribute rAttributeRow = xmlDocument.CreateAttribute("r");
                                rAttributeRow.Value = rowIndex.ToString();
                                rowNode.Attributes.Append(rAttributeRow);

                                XmlNode cNode = xmlDocument.CreateNode(XmlNodeType.Element, "c", null);
                                XmlAttribute rAttributeC = xmlDocument.CreateAttribute("r");
                                rAttributeC.Value = address;
                                cNode.Attributes.Append(rAttributeC);
                                XmlAttribute tAttributeC = xmlDocument.CreateAttribute("t");
                                tAttributeC.Value = "inlineStr";
                                cNode.Attributes.Append(tAttributeC);
                                rowNode.AppendChild(cNode);

                                XmlNode isNode = xmlDocument.CreateNode(XmlNodeType.Element, "is", null);
                                cNode.AppendChild(isNode);
                                XmlNode tNode = xmlDocument.CreateNode(XmlNodeType.Element, "t", null);
                                tNode.InnerText = "";
                                isNode.AppendChild(tNode);
                                lstSheetDataNode[0].InnerXml += rowNode.OuterXml;
                            }
                        }
                    }
                    entryData = xmlDocument.OuterXml;
                    entryStream.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(entryStream))
                    {
                        writer.Write(entryData);
                        writer.Flush();
                    }
                }
            }
            return true;
        }

        private string GetSheetId(string sheetName)
        {
            string sheetId = "";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            ZipArchiveEntry entry = ZipArchiveFile.Entries.Single(x => x.FullName == "xl/workbook.xml");
            using (Stream entryStream = entry.Open())
            {
                using (StreamReader reader = new StreamReader(entryStream))
                {
                    xmlDocument.LoadXml(reader.ReadToEnd());
                    XmlNodeList lstNode = xmlDocument.GetElementsByTagName("sheet");
                    if (lstNode != null && lstNode.Count > 0)
                    {
                        foreach (XmlElement node in lstNode)
                        {
                            if (node.Attributes["name"].Value == sheetName)
                            {
                                sheetId = node.Attributes["sheetId"].Value;
                            }
                        }
                    }
                }
            }
            return sheetId;
        }

        private int GetRowAddress(string cellAddress)
        {
            string strNumber = Regex.Replace(cellAddress, "[^0-9]", "");
            return Convert.ToInt32(strNumber);
        }

        private string GetColumnAddress(string cellAddress)
        {
            string strNumber = Regex.Replace(cellAddress.ToUpper(), "[^A-Z]", "");
            return strNumber;
        }
    }
}
