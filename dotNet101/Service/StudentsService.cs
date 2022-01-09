using dotNet101.Data;
using dotNet101.Model;
using dotNet101.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNet101.Service
{
    public class StudentsService : IStudentService
    {

        private readonly ApplicationDbContext _context;

        public StudentsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentById(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> UpdateStudent(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.StudentId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return student;
        }

        public async Task<Student> AddStudent(Student student)
        {
            if (string.IsNullOrEmpty(student.Name))
            {
                return null;
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            
            return student;
        }

        public async Task<object> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if(student is null)
            {
                return null;
            }
            
            var deletedStudent = _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return deletedStudent;
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
