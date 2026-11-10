
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public CategoriesController(RaceDayContext context)
        {
            _context = context;
        }

        // GET: api/categories
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .ToListAsync();

            return Ok(categories);
        }


        // POST: api/categories
        [HttpPost]
        [Authorize(Roles = "Organiser,Admin")]
        public async Task<IActionResult> CreateCategory(
            [FromBody] Category category)
        {
            // Step 1: Validate category information
            if (category == null)
            {
                return BadRequest(
                    "Category information is required.");
            }

            if (category.EventID <= 0)
            {
                return BadRequest(
                    "A valid Event ID is required.");
            }

            if (string.IsNullOrWhiteSpace(
                category.CategoryName))
            {
                return BadRequest(
                    "Category name is required.");
            }

            if (category.MaxParticipants <= 0)
            {
                return BadRequest(
                    "Maximum participants must be greater than zero.");
            }

            // Step 2: Find the selected event
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(
                    e => e.EventID == category.EventID);

            if (eventItem == null)
            {
                return NotFound(
                    "The selected event was not found.");
            }

            // Step 3: Get logged-in User ID from JWT
            string? userIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
            {
                return Unauthorized();
            }

            // Step 4: Check whether user is Admin
            bool isAdmin = User.IsInRole("Admin");

            // Step 5: Find Organiser profile
            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(
                    o => o.UserID == userId);

            // Step 6: Check event ownership
            if (!isAdmin)
            {
                if (organiser == null ||
                    organiser.OrganiserID != eventItem.OrganiserID)
                {
                    return Forbid();
                }
            }

            // Step 7: Reset Category ID
            // SQL Server generates the new ID
            category.CategoryID = 0;

            // Step 8: Save the new category
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            // Step 9: Return successful creation response
            return CreatedAtAction(
                nameof(GetCategory),
                new { id = category.CategoryID },
                category);
        }


        // PUT: api/categories/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser,Admin")]
        public async Task<IActionResult> UpdateCategory(
            int id, [FromBody] Category updatedCategory)
        {
            // Step 1: Validate the Category ID
            if (id <= 0)
            {
                return BadRequest("Invalid category ID.");
            }

            // Step 2: Check that request data was provided
            if (updatedCategory == null)
            {
                return BadRequest(
                    "Category information is required.");
            }

            // Step 3: Find the existing category
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound("Category was not found.");
            }

            // Step 4: Find the event that owns this category
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(
                    e => e.EventID == category.EventID);

            if (eventItem == null)
            {
                return NotFound(
                    "The category's event was not found.");
            }

            // Step 5: Get the logged-in User ID
            string? userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
            {
                return Unauthorized();
            }

            // Step 6: Check whether the user is an Admin
            bool isAdmin = User.IsInRole("Admin");

            // Step 7: Find the Organiser profile
            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(o => o.UserID == userId);

            // Step 8: Verify event ownership
            if (!isAdmin)
            {
                if (organiser == null ||
                    organiser.OrganiserID != eventItem.OrganiserID)
                {
                    return Forbid();
                }
            }

            // Step 9: Validate the new category details
            if (string.IsNullOrWhiteSpace(
                updatedCategory.CategoryName))
            {
                return BadRequest(
                    "Category name is required.");
            }

            if (updatedCategory.MaxParticipants <= 0)
            {
                return BadRequest(
                    "Maximum participants must be greater than zero.");
            }

            // Step 10: Update permitted properties only
            category.CategoryName =
                updatedCategory.CategoryName.Trim();

            category.Description =
                updatedCategory.Description;

            category.MaxParticipants =
                updatedCategory.MaxParticipants;

            // Do not update CategoryID or EventID

            // Step 11: Save changes to the database
            await _context.SaveChangesAsync();

            // Step 12: Return the updated category
            return Ok(category);
        }


        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser,Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // Step 1: Validate the Category ID
            if (id <= 0)
            {
                return BadRequest("Invalid category ID.");
            }

            // Step 2: Find the existing category
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound("Category was not found.");
            }

            // Step 3: Find the event linked to this category
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(
                    e => e.EventID == category.EventID);

            if (eventItem == null)
            {
                return NotFound(
                    "The category's event was not found.");
            }

            // Step 4: Get logged-in User ID from JWT
            string? userIdValue = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdValue, out int userId))
            {
                return Unauthorized();
            }

            // Step 5: Check whether the user is an Admin
            bool isAdmin = User.IsInRole("Admin");

            // Step 6: Find the Organiser profile
            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(o => o.UserID == userId);

            // Step 7: Verify ownership of the event
            if (!isAdmin)
            {
                if (organiser == null ||
                    organiser.OrganiserID != eventItem.OrganiserID)
                {
                    return Forbid();
                }
            }

            // Step 8: Check whether the category has enrolments
            bool hasEnrolments = await _context.Enrolments
                .AnyAsync(e => e.CategoryID == id);

            if (hasEnrolments)
            {
                return Conflict(
                    "This category cannot be deleted because " +
                    "participants are already enrolled in it.");
            }

            // Step 9: Delete the category
            _context.Categories.Remove(category);

            // Step 10: Save changes
            await _context.SaveChangesAsync();

            // Step 11: Return success
            return NoContent();
        }



        // GET: api/categories/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category ID.");
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound("Category was not found.");
            }

            return Ok(category);
        }
    }
}
