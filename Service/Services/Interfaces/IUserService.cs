using Domain.Entities;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserLoginModel> Login(PostLoginModel model);

        Task<UserLoginModel> Post(PostUserModel model);

        Task<User> Get(long id);
    }
}
