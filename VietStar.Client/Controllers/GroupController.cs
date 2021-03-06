using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.GroupModels;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Route("Groups")]
    public class GroupController : VietStarBaseController
    {
        protected readonly IGroupBusiness _bizGroup;
        public GroupController(IGroupBusiness groupBusiness, CurrentProcess process) : base(process)
        {
            _bizGroup = groupBusiness;
        }


        [MyAuthorize(Permissions = "group")]
        [Route("Index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [MyAuthorize(Permissions = "group,group.read")]
        [HttpGet("GroupsByUserId")]
        public async Task<IActionResult> GetGroupsByUserId()
        {
            var result = await _bizGroup.GetGroupByUserId();
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "group,group.read")]
        [HttpGet("ApproveGroupsByUserId")]
        public async Task<IActionResult> GetApproveGroupsByUserId()
        {
            var result = await _bizGroup.GetApproveGroupByUserId();
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "group,group.read")]
        [HttpGet("ParentGroups")]
        public async Task<IActionResult> GetParentGroups()
        {
            var result = await _bizGroup.GetParentGroups();
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "group,group.read")]
        [HttpGet("Childrens")]
        public async Task<IActionResult> GetChildByParentGroupId(int parentId, int page = 1, int limit = 10)
        {
            var result = await _bizGroup.SearchAsync(parentId, page, limit);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "group,group.write")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _bizGroup.GetGroupByIdAsync(id);
            return View(result);
        }

        [MyAuthorize(Permissions = "group,group.write")]
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GroupEditModel model)
        {
            var result = await _bizGroup.UpdateAsync(model);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "group,group.read")]
        [HttpGet("members/{groupId}")]
        public async Task<IActionResult> GetMembers(int groupId)
        {
            var result = await _bizGroup.GetMemberByGroupIdAsync(groupId);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "group,group.write")]
        [HttpGet("Config")]
        public IActionResult Config()
        {
            return View();
        }

        [MyAuthorize(Permissions = "group,group.write")]
        [HttpPost("CreateConfig")]
        public async Task<IActionResult> CreateConfig([FromBody]CreateConfigModel model)
        {
            var result = await _bizGroup.CreateConfigAsync(model);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "group,group.write")]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [MyAuthorize(Permissions = "group,group.write")]
        [HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroup([FromBody]GroupCreateModel model)
        {
            var result = await _bizGroup.CreateAsync(model);
            return ToResponse(result);
        }

    }
}