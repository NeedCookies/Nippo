﻿using Application.Contracts;
using Application.Contracts.Operations;
using Domain.Entities;
using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface ICoursesService
    {
        public Task<List<Course>> GetAllCourses();
        public Task<Course> Create(CreateCourseRequest request, string authorId);
        public Task<Course> Update(UpdateCourseRequest request);
        public Task<Course> Delete(int id);
        public Task<Course> GetById(int id);
        public Task<string> GetAuthorById(int id);
        public Task<int> ApplyPromocode(CoursePurchaseRequest request);
        public Task<UserCourses> PurchaseCourse(CoursePurchaseRequest request, string userId);
        public Task<Course> SubmitForReview(int courseId, string userId);
        public Task<List<Course>> GetCoursesToCheck();
        public Task<Course> AcceptCourse(int courseId);
        public Task<Course> CancelCourse(int courseId);
        public Task<BasketCourses> AddToBasket(int courseId, string userId);
        public Task<BasketCourses> DeleteFromBasket(int courseId, string userId);
        public Task<List<BasketCourses>> GetBasketCourses(string userId);
    }
}
