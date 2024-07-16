using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Operations;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("course")]
    public class CourseController(
            ICoursesService coursesService,
            UserManager<ApplicationUser> userManager,
            IUserProgressService userProgressService) : ControllerBase
    {
        [HttpGet("get-all-courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var allCourses = await coursesService.GetAllCourses();

            return Ok(allCourses);
        }

        [HttpGet("get-course")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await coursesService.GetById(id);
            
            if (course == null)
            {
                return BadRequest("No such id");
            }

            return Ok(course);
        }

        [Authorize(Roles = "author")]
        [HttpPost("create-course")]
        public async Task<IActionResult> Create([FromForm] CreateCourseRequest request)
        {
            string userId = GetUserId();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with id '{userId}' not found.");
                // Или выполняем другие действия по обработке ошибки, например, возвращаем NotFound или BadRequest
            }

            var course = await coursesService.Create(request, userId);
            
            return Ok(course);
        }

        [Authorize]
        [HttpPost("purchase-course")]
        public async Task<IActionResult> PurchaseCourse([FromBody] CoursePurchaseRequest request)
        {
            string userId = GetUserId();

            var result = await coursesService.PurchaseCourse(request, userId);
            
            return Ok(result);
        }

        [Authorize]
        [HttpPost("apply-promocode")]
        public async Task<IActionResult> ApplyPromocode([FromBody] CoursePurchaseRequest request)
        {
            var newPrice = await coursesService.ApplyPromocode(request);
            return Ok(newPrice);
        }

        [Authorize]
        [HttpPost("add-to-basket")]
        public async Task<IActionResult> AddToBasket(int courseId)
        {
            string userId = GetUserId();

            var result = await coursesService.AddToBasket(courseId, userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("remove-from-basket")]
        public async Task<IActionResult> RemoveFromBasket(int courseId)
        {
            string userId = GetUserId();

            var result = await coursesService.DeleteFromBasket(courseId, userId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-basket-courses")]
        public async Task<IActionResult> GetBasketCourses()
        {
            string userId = GetUserId();

            var result = await coursesService.GetBasketCourses(userId);
            return Ok(result);
        }

        [Authorize(Roles = "author")]
        [HttpPost("edit-course")]
        public async Task<IActionResult> Update([FromForm] UpdateCourseRequest request)
        {
            var course = await coursesService.Update(request);
            
            return Ok(course);
        }

        [Authorize(Roles = "author")]
        [HttpDelete("delete-course")]
        public async Task<IActionResult> Delete(int courseId)
        {
            var course = await coursesService.Delete(courseId);
            
            return Ok(course);
        }

        [Authorize(Roles ="author")]
        [HttpPost("submit-for-review")]
        public async Task<IActionResult> SubmitForReview([FromBody]SubmitCourse courseSubmit)
        {
            int courseId = courseSubmit.courseId;
            string userId = GetUserId();
            var course = await coursesService.SubmitForReview(courseId, userId);
            
            return Ok(course);
        }

        [HttpGet("get-course-progress")]
        public async Task<IActionResult> GetCourseProgress(int courseId)
        {
            GetCompletedElementsRequest request = new GetCompletedElementsRequest 
                (
                GetUserId(), 
                courseId
                );
            
            var courseProgress = await userProgressService.GetCourseProgress(request);

            return Ok(courseProgress);
        }

        [HttpGet("get-course-element-status")]
        public async Task<IActionResult> GetCourseElementStatus(int courseId, int elementId, int elementType)
        {
            UserProgressRequest request = new UserProgressRequest
                (
                GetUserId(),
                courseId,
                elementId,
                elementType
                );

            return Ok(await userProgressService.GetCourseElementStatus(request));
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirst("userId")!.Value;
        }
    }
}
