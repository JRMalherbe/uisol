using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UISWeb.Data;
using UISWeb.Models;

namespace UISWeb.Controllers
{
    public class CustomerRequestController : Controller
    {
        private readonly UISWebContext _context;

        public CustomerRequestController(UISWebContext context)
        {
            _context = context;
        }

        // GET: CustomerRequest
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomerRequest.ToListAsync());
        }

        // GET: CustomerRequest/Details/5
        public async Task<IActionResult> Details(int? id)
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
