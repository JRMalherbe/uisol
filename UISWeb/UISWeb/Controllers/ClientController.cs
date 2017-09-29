using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UISWeb.Data;
using UISWeb.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace UISWeb.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly UISWebContext _context;
        private readonly UISConfig _config;
        private static HttpClient _client = new HttpClient();
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(
            UISWebContext context, 
            UserManager<ApplicationUser> userManager, 
            IOptions<UISConfig> config)
        {
            _context = context;
            _config = config.Value;
            _userManager = userManager;
        }

        // GET: Client
        public async Task<IActionResult> Index()
        {
            List<Client> clientList = new List<Client>();

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            bool IsAdmin = currentUser.IsInRole("Admin");
            //var id = _userManager.GetUserId(this.User); // Get user id:
            var user = await _userManager.GetUserAsync(this.User);
            var email = user.Email;

            string url = _config.ServiceRoot + "api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(email));
            if (IsAdmin)
                url = _config.ServiceRoot + "api/Customer";
            HttpResponseMessage result = _client.GetAsync(url).Result;
            string body = "";
            if (result.IsSuccessStatusCode)
            {
                body = result.Content.ReadAsStringAsync().Result;
                if (IsAdmin)
                {
                    clientList = JsonConvert.DeserializeObject<List<Client>>(body);
                }
                else
                {
                    Client client = JsonConvert.DeserializeObject<Client>(body);
                    clientList.Add(client);
                }
            }
            return View(clientList);
            //return View(await _context.Client.ToListAsync());
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .SingleOrDefaultAsync(m => m.Email == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,ContactName,CompanyName,ClientId")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.SingleOrDefaultAsync(m => m.Email == id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,ContactName,CompanyName,ClientId")] Client client)
        {
            if (id != client.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Email))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .SingleOrDefaultAsync(m => m.Email == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = await _context.Client.SingleOrDefaultAsync(m => m.Email == id);
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
            return _context.Client.Any(e => e.Email == id);
        }
    }
}
