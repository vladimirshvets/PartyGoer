using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebInterface.Data;
using WebInterface.Models;

namespace WebInterface.Pages.Chats
{
    public class IndexModel : PageModel
    {
        private readonly WebInterface.Data.ApplicationDbContext _context;

        public IndexModel(WebInterface.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Chat> Chat { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Chats != null)
            {
                Chat = await _context.Chats.ToListAsync();
            }
        }
    }
}
