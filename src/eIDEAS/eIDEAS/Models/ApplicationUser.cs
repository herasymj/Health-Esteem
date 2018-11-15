using eIDEAS.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eIDEAS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int DivisionID { get; set; }

        [Required]
        public int UnitID { get; set; }

        public int IdeaPoints { get; set; }

        public int ParticipationPoints { get; set; }

        public int Permissions { get; set; }

        public string ProfilePic { get; set; }

        public DateTime? DateDeleted { get; set; }


        //Find out if user is in role
        public bool IsRole(RoleEnum roleName)
        {
            int permission = this.Permissions;
            int role = (int)roleName;

            return (0b1 & (permission >> (role - 1))) == 1; //Using bit shifting, find out if user has role
        }

        //Return list of roles for current user
        public List<RoleEnum> Roles()
        {
            var roleList = new List<RoleEnum>();

            int numOfRoles = Enum.GetNames(typeof(RoleEnum)).Length;
            for(int i = 1; i <= numOfRoles; i++)
            {
                if (IsRole((RoleEnum)i))
                {
                    roleList.Add((RoleEnum)i);
                }
            }

            return roleList;
        }
    }
}
