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
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace UISWeb.Controllers
{
    public class CustomerRequestController : Controller
    {
        private readonly UISWebContext _context;
        private readonly UISConfig _config;
        private static HttpClient _client = new HttpClient();
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerRequestController(
            UISWebContext context,
            UserManager<ApplicationUser> userManager,
            IOptions<UISConfig> config)
        {
            _context = context;
            _config = config.Value;
            _userManager = userManager;
        }

        // GET: CustomerRequest
        public async Task<IActionResult> Index()
        {
            List<CustomerRequest> customerRequests = new List<CustomerRequest>();

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            bool IsAdmin = currentUser.IsInRole("Admin");
            var id = _userManager.GetUserId(this.User); // Get user id:
            var user = await _userManager.GetUserAsync(User);
            var email = user.Email;

            string url = _config.ServiceRoot + "api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(email)) + "/Reports";
            //if (IsAdmin)
            //    url = _config.ServiceRoot + "api/Customer/" + email + "/Reports";
            HttpResponseMessage result = _client.GetAsync(url).Result;
            string body = "";
            if (result.IsSuccessStatusCode)
            {
                body = result.Content.ReadAsStringAsync().Result;
                customerRequests = JsonConvert.DeserializeObject<List<CustomerRequest>>(body);
            }

            return View(customerRequests);
            //return View(await _context.CustomerRequest.ToListAsync());
        }

        // GET: CustomerRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            CustomerRequest customerRequest = null;

            if (id == null)
            {
                return NotFound();
            }

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            bool IsAdmin = currentUser.IsInRole("Admin");
            //var userId = _userManager.GetUserId(this.User); // Get user id:
            var user = await _userManager.GetUserAsync(this.User);
            var email = user.Email;

            string url = "";
            string body = "";
            HttpResponseMessage result = null;
            url = _config.ServiceRoot + "api/Customer/" + Convert.ToBase64String(Encoding.ASCII.GetBytes(email)) + "/Reports/" + id.ToString();
            //if (IsAdmin)
            //    url = _config.ServiceRoot + "api/Customer/" + email + "/Reports/" + labno.ToString();
            result = _client.GetAsync(url).Result;
            body = "";
            if (result.IsSuccessStatusCode)
            {
                // Remove request - from local db
                CustomerRequest req = _context.CustomerRequest.Find(id.Value);
                if (req != null)
                {
                    _context.CustomerRequest.Remove(req);
                    _context.SaveChanges();
                }

                // Replace request - from access db
                body = result.Content.ReadAsStringAsync().Result;
                customerRequest = JsonConvert.DeserializeObject<CustomerRequest>(body);
                _context.CustomerRequest.Add(customerRequest);
                _context.SaveChanges();

            }

            if (customerRequest == null)
            {
                return NotFound();
            }

            return View(customerRequest);
        }

        public FileResult Download(string link, int labno)
        {
            CustomerFile customerFile = _context.CustomerFile.Where(x => x.CustomerRequestLabNo == labno && x.LinkName == link).SingleOrDefault();
            //CustomerFile customerFile = _context.CustomerFile.Where(x => x.LinkName == link).SingleOrDefault();
            string fileName = "Not Found.pdf";
            if (customerFile != null)
                fileName = customerFile.FileName;
            string filePath = _config.ReportPath + fileName;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // GET: CustomerRequest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerRequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabNo,CustomerId,Coordinator,Detail,Received,Required,Completed,Invoiced")] CustomerRequest customerRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerRequest);
        }

        // GET: CustomerRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerRequest = await _context.CustomerRequest.SingleOrDefaultAsync(m => m.LabNo == id);
            if (customerRequest == null)
            {
                return NotFound();
            }
            return View(customerRequest);
        }

        // POST: CustomerRequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LabNo,CustomerId,Coordinator,Detail,Received,Required,Completed,Invoiced")] CustomerRequest customerRequest)
        {
            if (id != customerRequest.LabNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerRequestExists(customerRequest.LabNo))
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
            return View(customerRequest);
        }

        // GET: CustomerRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerRequest = await _context.CustomerRequest
                .SingleOrDefaultAsync(m => m.LabNo == id);
            if (customerRequest == null)
            {
                return NotFound();
            }

            return View(customerRequest);
        }

        // POST: CustomerRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerRequest = await _context.CustomerRequest.SingleOrDefaultAsync(m => m.LabNo == id);
            _context.CustomerRequest.Remove(customerRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerRequestExists(int id)
        {
            return _context.CustomerRequest.Any(e => e.LabNo == id);
        }
    }
}
