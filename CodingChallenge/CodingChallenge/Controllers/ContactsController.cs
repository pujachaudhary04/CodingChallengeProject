using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodingChallenge.Data;
using CodingChallenge.Models;
using System.Data;

namespace CodingChallenge.Controllers
{
    public class ContactsController : Controller
    {
        private readonly CodingChallengeContext _context;

        public ContactsController(CodingChallengeContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            return _context.Contact != null ? 
                          View(await _context.Contact.ToListAsync()) :
                          Problem("Entity set 'CodingChallengeContext.Contact'  is null.");

            }

        public async Task<ActionResult> PartialData()
        {
            List<Contact> contacts = await _context.Contact.ToListAsync();
            List<Address> addresses = await _context.Address.ToListAsync();

            //Address
            DataTable dt_address = new DataTable();
            dt_address.Columns.Add("CombinedColumn");
            foreach (var item in addresses)
            {
                var row = dt_address.NewRow();
                row["CombinedColumn"] = item.Street + ", " + item.City + ", " + item.State + ", " + Convert.ToString(item.ZipCode);

                dt_address.Rows.Add(row);
            }

            //Contact
            DataTable dt_contact = new DataTable();
            dt_contact.Columns.Add("Id");
            dt_contact.Columns.Add("FirstName");
            dt_contact.Columns.Add("LastName");
            dt_contact.Columns.Add("Address");

            foreach (var item in contacts)
            {
                var row = dt_contact.NewRow();
                row["Id"] = item.Id;
                row["FirstName"] = item.FirstName;
                row["LastName"] = item.LastName;

                dt_contact.Rows.Add(row);
            }

            for (int i = 0; i < dt_contact.Rows.Count; i++)
            {
                dt_contact.Rows[i][3] = dt_address.Rows[i][0];
            }

            List<Combined> contactList = new List<Combined>();
            contactList = (from DataRow dr in dt_contact.Rows
                           select new Combined()
                           {
                               Id = Convert.ToInt32(dr["Id"]),
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString(),
                               Address = dr["Address"].ToString()
                           }).ToList();

            return View(contactList);
        }



        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contact == null)
            {
                return Problem("Entity set 'CodingChallengeContext.Contact'  is null.");
            }
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
          return (_context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
