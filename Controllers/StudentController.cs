using System;
using Microsoft.AspNetCore.Mvc;
using SampleSecureWeb.Data;
using SampleSecureWeb.Models;

namespace SampleSecureWeb.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudent _studentData;
        public StudentController(IStudent studentData)
        {
            _studentData = studentData;
        }
        public IActionResult index()
        {
            var student = _studentData.GetStudents();
            return View(student); 
        }

        public IActionResult create()
        {
            return View(); 
        }
        [HttpPost]
        public IActionResult create(Student student)
        {
            try
            {
                _studentData.AddStudent(student);
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(student);
        }

    }
}