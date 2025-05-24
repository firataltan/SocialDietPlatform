using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApp.Application.Commands;
using RecipeApp.Application.Services;
using RecipeApp.Domain.Entities;
using RecipeApp.Domain.Interfaces;

namespace RecipeApp.Application.Handlers
{
    public class AddCommentCommandHandler
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<AddCommentCommandHandler> _logger;

        public AddCommentCommandHandler(ICommentRepository commentRepository, ILogger<AddCommentCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("api/comments")]
        public async Task<IActionResult> Handle(AddCommentCommand request)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;

                var comment = new Comment
                {
                    Content = request.Content,
                    UserId = userId,
                    PostId = request.RecipeId,
                    RecipeId = request.RecipeId
                };

                await _commentRepository.AddAsync(comment);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a comment.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
} 