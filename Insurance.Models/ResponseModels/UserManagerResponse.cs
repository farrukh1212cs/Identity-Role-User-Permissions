using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Models.ResponseModels
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public DateTime? ExpireDate { get; set; }
    }
}
