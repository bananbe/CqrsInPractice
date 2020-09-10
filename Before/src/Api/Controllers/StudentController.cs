using System;
using System.Collections.Generic;
using Logic.Dtos;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly Messages _messages;
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public StudentController(UnitOfWork unitOfWork, Messages messages)
        {
            _unitOfWork = unitOfWork;
            _messages = messages;
            _studentRepository = new StudentRepository(unitOfWork);
            _courseRepository = new CourseRepository(unitOfWork);
        }

        [HttpGet]
        public IActionResult GetList(string enrolled, int? number)
        {
           List<StudentDto> list = _messages.Dispatch(new GetListQuery(enrolled, number));
           return Ok(list);
        }
        
        [HttpPost]
        public IActionResult Register([FromBody] NewStudentDto dto)
        {

            var command = new RegisterCommand(dto.Name, dto.Email, dto.Course1, dto.Course1Grade, dto.Course2, dto.Course2Grade);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpDelete("{id}")]
        public IActionResult Unregister(long id)
        {
            var command = new UnregisterCommand(id);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, [FromBody] StudentEnrollmentDto dto)
        {
            var command = new EnrollCommand(id, dto.Course, dto.Grade);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        public IActionResult Transfer(long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
        {
           var command = new TransferCommand(id, enrollmentNumber, dto.Course, dto.Course);
           var result = _messages.Dispatch(command);

           return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPost("{id}/enrollments/{enrollmentNumber}/deletion")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
        {
            var command = new DisenrollCommand(id, enrollmentNumber, dto.Comment);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(long id, [FromBody] StudentDto dto)
        {
            var command = new EditPersonalInfoCommand(dto.Id, dto.Name, dto.Email);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }
    }
}
