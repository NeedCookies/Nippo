﻿using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;

namespace Application.Services
{
    public class UserProgressService(
            ILessonRepository lessonRepository, 
            IQuizRepository quizRepository,
            IUserProgressRepository userProgressRepository) : IUserProgressService
    {
        public async Task<CompletedElementsCourseResponse> GetCourseProgress(GetCompletedElementsRequest request)
        {
            var courseLessons = await lessonRepository.GetLessonsSizeByCourse(request.courseId);
            var courseQuizzes = await quizRepository.GetQuizzesSizeByCourse(request.courseId);

            var completedElements = await userProgressRepository.GetCompletedCourses(request.userId, request.courseId);

            CompletedElementsCourseResponse completed = new CompletedElementsCourseResponse
            (
                completedElements,
                courseLessons + courseQuizzes
            );

            return completed;
        }

        public async Task<bool> GetCourseElementStatus(UserProgressRequest userProgress) =>
            await userProgressRepository.GetElementStatus(userProgress);
    }
}
