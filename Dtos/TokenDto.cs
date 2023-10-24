using System.ComponentModel;
using System;

namespace RefactorThis.Dtos
{
    public class TokenDto
    {
        public Guid APIToken { get; set; }
        public string Message { get; set; }
    }
}
