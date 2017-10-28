using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UISWeb.Data;
using UISWeb.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace UISWeb.Controllers
{
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
            return View(await _context.Client.ToListAsync());
        }

        public async Task<IActionResult> Refresh()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            bool IsAdmin = currentUser.IsInRole("Admin");
            if (!IsAdmin)
                return View("Index", await _context.Client.ToListAsync());

            List<Client> clientList = new List<Client>();
            string url = _config.ServiceRoot + "api/Customer";
            HttpResponseMessage result = _client.GetAsync(url).Result;
            string body = "";
            if (result.IsSuccessStatusCode)
            {
                body = result.Content.ReadAsStringAsync().Result;
                clientList = JsonConvert.DeserializeObject<List<Client>>(body);

                foreach (Client client in clientList)
                {
                    Client tc = _context.Client.Find(client.ClientId);
                    if (tc == null)
                    {
                        await _context.Client.AddAsync(client);
                    }

                    if (!String.IsNullOrEmpty(client.Email))
                    {
                        User user = _context.User.Find(client.Email);
                        if (user == null)
                        {
                            user = new Models.User() { Email = client.Email };
                            await _context.User.AddAsync(user);
                            await _context.ClientUser.AddAsync(new ClientUser() { Client = client, User = user });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                //await _context.SaveChangesAsync();
            }
            //return View("Index", clientList);

            return View("Index", await _context.Client.ToListAsync());
        }


        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .Include("Users")
                .SingleOrDefaultAsync(m => m.ClientId == id);
                
            if (client == null)
            {
                return NotFound();
            }

            ClientUser viewModel = new ClientUser() { Client = client, User = new Models.User() };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(ClientUser clientUser)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.User.SingleOrDefaultAsync(u => u.Email == clientUser.UserEmail);
                if (user == null)
                {
                    user = new Models.User() { Email = clientUser.UserEmail };
                    _context.User.Add(user);
                    await _context.SaveChangesAsync();
                }

                ClientUser cu = await _context.ClientUser.SingleOrDefaultAsync(c => c.UserEmail == clientUser.UserEmail && c.ClientId == clientUser.Client.ClientId);
                if (cu == null)
                {
                    cu = new Models.ClientUser() { UserEmail = clientUser.UserEmail, ClientId = clientUser.Client.ClientId };
                    _context.ClientUser.Add(cu);
                    await _context.SaveChangesAsync();
                }

                Client client = await _context.Client
                                .Include("Users")
                                .SingleOrDefaultAsync(m => m.ClientId == clientUser.Client.ClientId);

                ClientUser viewModel = new ClientUser() { Client = client, User = new Models.User() };

                return View("Details", viewModel);
            }

            return View("Details",clientUser);
        }

        public async Task<IActionResult> RemoveEmail(string userEmail, int clientId)
        {
            if (userEmail == null)
            {
                return NotFound();
            }

            ClientUser cu = await _context.ClientUser
                .SingleOrDefaultAsync(m => m.UserEmail == userEmail && m.ClientId == clientId);
            if (cu == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(cu);
                await _context.SaveChangesAsync();
            }

            Client client = await _context.Client
                            .Include("Users")
                            .SingleOrDefaultAsync(m => m.ClientId == clientId);

            ClientUser viewModel = new ClientUser() { Client = client, User = new Models.User() };

            return View("Details", viewModel);
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
        public async Task<IActionResult> Create([Bind("ClientId,Email,ContactName,CompanyName")] Client client)
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.SingleOrDefaultAsync(m => m.ClientId == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,Email,ContactName,CompanyName")] Client client)
        {
            if (id != client.ClientId)
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
                    if (!ClientExists(client.ClientId))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .SingleOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Client.SingleOrDefaultAsync(m => m.ClientId == id);
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.ClientId == id);
        }
    }
}
