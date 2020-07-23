using BuyerProfile.Models;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.InterFace;
using Service;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BuyerProfile.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AffiliateSellController : Controller
    {
        private readonly IUnitOfWork<BuyerDbContext> _uow;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AffiliateSellController> _logger;
        private readonly IEmailSender _mailSender;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _Config;

        public AffiliateSellController(IUnitOfWork<BuyerDbContext> uow,
            UserManager<ApplicationUser> userManager,
            ILogger<AffiliateSellController> logger,
            IHttpClientFactory clientFactory,
            IEmailSender mailSender,
            IConfiguration Config)
        {
            _uow = uow;
            _userManager = userManager;
            _logger = logger;
            _mailSender = mailSender;
            _clientFactory = clientFactory;
            _Config = Config;
        }


        /// <summary>
        /// if buyer is not available make new account 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IActionResult> CheckBuyerByEmail([FromQuery]string email)
        {
            try
            {
                HttpContext.Response.ContentType = "application/json";

                var affiliateUser = await _uow.UserRepo.GetAsync(d => d.Email == email);
                if (!affiliateUser.Any())
                {
                    var user = new ApplicationUser { UserName = email, Email = email, Name = email.Split('@')[0] };
                    user.IsActive = true;
                    var passwordGenerated = Guid.NewGuid().ToString().Substring(0, 6);
                    var result = await _userManager.CreateAsync(user, passwordGenerated);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");
                        var message = string.Format("<p>Your account username: {0} </p><p>Your account password: {1} </p>", email, passwordGenerated);
                        try
                        {
                            await _mailSender.SendEmailAsync(email, "buyer created successfully", message);
                        }
                        catch (Exception ex)
                        {
                            var emailResult = "User created but did not send email account \n" + ex.Message;
                            return Ok(emailResult);
                        }
                    }
                    else
                    {
                        return BadRequest(result.Errors.FirstOrDefault());
                    }
                }
                return Ok("This user is available now");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// test above api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SendRequestToBuyerProfile()
        {
            var url = _Config["baseUrl"] + "/api/AffiliateSell/CheckBuyerByEmail?email=";
            url += "reza.@gmail.com";
            var client = _clientFactory.CreateClient();

            HttpResponseMessage messages = await client.GetAsync(url);
            HttpContext.Response.ContentType = "application/json";

            if (messages.IsSuccessStatusCode)
            {
                var result = JsonConvert.SerializeObject((await messages.Content.ReadAsStringAsync()));
                return Ok(result);
            }
            return BadRequest();
        }

    }
}