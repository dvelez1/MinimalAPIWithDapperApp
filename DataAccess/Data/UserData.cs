using DataAccess.DbAccess;
using DataAccess.Models;
using DataAccess.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _db;

    public UserData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<IEnumerable<UserModel>> GetUsers() =>
        _db.LoadData<UserModel, dynamic>("dbo.spUser_GetAll", new { });

    public async Task<UserModel?> GetUser(int id)
    {
        var results = await _db.LoadData<UserModel, dynamic>(
            "dbo.spUser_Get",
            new { Id = id });
        return results.FirstOrDefault();
    }

    public Task InsertUser(UserModel user) =>
        _db.SaveData("dbo.spUser_Insert", new { user.FirstName, user.LastName });

    public Task UpdateUser(UserModel user) =>
        _db.SaveData("dbo.spUser_Update", user);

    public Task DeleteUser(int id) =>
        _db.SaveData("dbo.spUser_Delete", new { Id = id });


    // Improvement Methods
    public async Task<UserModel?> GetUserWithDynamicParameters(UserModel userModel)
    {
        var results = await _db.LoadDataWithDynamicParameters<UserModel>("dbo.spUser_Get", DynamicDapperParametersMapper.DynamicParametersMapper(new {userModel.Id}));

        return results.FirstOrDefault();
    }

    public Task InsertUserWithDynamicParameters(UserModel userModel) => _db.SaveDataWithDynamicParameters<UserModel>("dbo.spUser_Insert",
        DynamicDapperParametersMapper.DynamicParametersMapper(new { userModel.FirstName, userModel.LastName }));
   
    
}
