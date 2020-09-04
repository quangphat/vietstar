using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.Note;
using VietStar.Repository.Interfaces;

namespace VietStar.Repository
{
    public class NoteRepository : RepositoryBase, INoteRepository
    {
        public NoteRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task AddNoteAsync(NoteAddModel model)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("insert into Ghichu (UserId,Noidung,HosoId, CommentTime,TypeId) values(@userId,@noidung,@hosoId,@commentTime,@typeId)",
                    new
                    {
                        userId = model.UserId,
                        noidung = model.Content,
                        hosoId = model.ProfileId,
                        commentTime = DateTime.Now,
                        typeId = model.ProfileTypeId
                    }, commandType: CommandType.Text);
            }
        }

        public async Task<List<NoteViewModel>> GetNoteByTypeAsync(int profileId, int profileTypeId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<NoteViewModel>("sp_GetGhichuByHosoId_v2",
                    new
                    {
                        profileId,
                        profileTypeId
                    }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}
