﻿using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Loads a user based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> LoadUserById(Guid id);

        /// <summary>
        /// Loads a user based in name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<User> LoadUserByName(string name);

        /// <summary>
        /// Saves a user based on username and password. Generates a id while saving
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task SaveUser(string name);

        /// <summary>
        /// Get all saved users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAllUsers();
    }
}