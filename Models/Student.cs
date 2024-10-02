using System;
using System.ComponentModel.DataAnnotations;

namespace SampleSecureWeb.Models
{
    public class Student
    {
        [Key]
        public string Nim{ get; set;} = null!;
        public string namalengkap{ get; set;} = null!;

    }
}