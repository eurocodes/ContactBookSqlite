using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBook.DataAccess.Interfaces;
using ContactBook.ModelDto;
using ContactBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IContactRepository _contactRepository;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IContactRepository contactRepository, IMapper mapper, IConfiguration config)
        {
            _userRepository = userRepository;
            _contactRepository = contactRepository;
            _mapper = mapper;

            Account account = new Account
            {
                Cloud = config.GetSection("CloudinarySettings:CloudName").Value,
                ApiKey = config.GetSection("CloudinarySettings:ApiKey").Value,
                ApiSecret = config.GetSection("CloudinarySettings:ApiSecret").Value,
            };

            _cloudinary = new Cloudinary(account);
        }

        [Authorize(Policy = "AdminRolePolicy")]
        [HttpPost("add-new")]
        public async Task<IActionResult> AddContact(AddContactDto addContact)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var addressId = Guid.NewGuid().ToString();
            var contactToAdd = new Contact
            {
                FirstName = addContact.FirstName,
                LastName = addContact.LastName,
                Email = addContact.Email,
                PhoneNumber = addContact.PhoneNumber,
                Photo = addContact.PhotoUrl,
                UserId = userId,
                Address = new Address
                {
                    HouseNumber = addContact.HouseNumber,
                    Street = addContact.Street,
                    City = addContact.City,
                    State = addContact.State,
                    Country = addContact.Country,
                }
                
            };
            var added = await _contactRepository.AddContact(contactToAdd);
            if (added)
                return Created("", contactToAdd);

            return BadRequest();
        }

        [Authorize(Policy = "AdminRolePolicy")]
        [HttpGet("all-users/{page?}")]
        public IActionResult GetAllContacts(string page)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            page ??= "1";
            var contacts = _contactRepository.GetAllContacts(int.Parse(page), userId);
            if (contacts.Count < 1)
                return NoContent();

            // var contactsToReturn = _mapper.Map<ICollection<AddContactDto>>(contacts);
            return Ok(contacts);
        }

        [Authorize(Policy = "AdminRolePolicy, RegularRolePolicy")]
        [HttpGet("{userId}")]
        public IActionResult GetSingleContactById(string userId)
        {
            var contacts = _contactRepository.GetContactById(userId);
            if(contacts == null)
                return NotFound();

            return Ok(contacts);
        }

        [Authorize(Policy = "AdminRolePolicy, RegularRolePolicy")]
        [HttpGet("{email}")]
        public IActionResult GetSingleContactsByEmail(string email)
        {
            var contacts = _contactRepository.GetContactByEmail(email);
            if (contacts == null)
                return NotFound();

            return Ok(contacts);
        }

        [Authorize(Policy = "AdminRolePolicy")]
        [HttpGet("search-term/{searchterm}")]
        public async Task<IActionResult> GetSingleContactsBySearch(string searchterm)
        {
            var contacts = await _contactRepository.Search(searchterm);
            if (contacts == null)
                return NotFound("No record found");

            return Ok(contacts);
        }

        [Authorize(Policy = "AdminRolePolicy, RegularRolePolicy")]
        [HttpPut("update/{userId}")]
        public IActionResult UpdateContacts(string userId, UpdateContactDto addContact)
        {
            var contacts = _contactRepository.GetContactById(userId);
            if (contacts == null)
                return NotFound();

            var contactToAdd = new Contact
            {
                Id = contacts.Id,
                UserId = contacts.UserId,
                FirstName = addContact.FirstName?? contacts.FirstName,
                LastName = addContact.LastName?? contacts.LastName,
                Email = addContact.Email?? contacts.Email,
                PhoneNumber = addContact.PhoneNumber?? contacts.PhoneNumber,
                Photo = addContact.PhotoUrl?? contacts.Photo,
                Address = new Address
                {
                    HouseNumber = addContact.HouseNumber ?? contacts.Address.HouseNumber,
                    Street = addContact.Street ?? contacts.Address.Street,
                    City = addContact.City ?? contacts.Address.City,
                    State = addContact.State ?? contacts.Address.State,
                    Country = addContact.Country ?? contacts.Address.Country,
                    Id = contacts.Address.Id,
                }

            };
            var added = _contactRepository.UpdateContact(contactToAdd);
            if (added)
                return Ok(contactToAdd);

            return BadRequest();
        }

        [Authorize(Policy = "AdminRolePolicy")]
        [HttpDelete("delete/{userid}")]
        public async Task<IActionResult> DeleteContact(string userid)
        {
            var contacts = await _contactRepository.DeleteContact(userid);
            if (!contacts)
                return NotFound(userid);

            return NoContent();
        }

        [Authorize(Policy = "AdminRolePolicy, RegularPolicy")]
        [HttpPatch("upload/{userid}")]
        public IActionResult AttachPhoto(string userid, [FromForm] PhotoToAddDto model)
        {
            var contacts = _contactRepository.GetContactById(userid);
            if (contacts == null)
                return NotFound();

            var file = model.PhotoFile;
            if (file.Length <= 0)
                return BadRequest("Invalid file size");

            var imageUploadResult = new ImageUploadResult();

            using (var fs = file.OpenReadStream())
            {
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, fs),
                    Transformation = new Transformation().Width(300).Height(300).Crop("fill").Gravity("face")
                };
                imageUploadResult = _cloudinary.Upload(imageUploadParams);
            }

            var publicId = imageUploadResult.PublicId;
            var Url = imageUploadResult.Url.ToString();
            contacts.Photo = Url;

            var added = _contactRepository.UpdateContact(contacts);
            if (added)
                return Ok(contacts);

            return BadRequest();
        }

    }
}
