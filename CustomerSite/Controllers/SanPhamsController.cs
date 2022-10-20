﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedModel.Models;

namespace CustomerSite.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly ILogger<SanPhamsController> _logger;
        private readonly qlbanhangContext _context=new qlbanhangContext();

        public SanPhamsController(ILogger<SanPhamsController> logger)
        {
            _logger = logger;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
            var qlbanhangContext = _context.SanPhams.Include(s => s.MaLoaiSpNavigation);
            return View(await qlbanhangContext.ToListAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qlbanhangContext = _context.SanPhams.Include(s => s.MaLoaiSpNavigation).FirstOrDefault(s=>s.MaSp==id);
            return View( qlbanhangContext);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            ViewData["MaLoaiSp"] = new SelectList(_context.LoaiSps, "MaLoaiSp", "MaLoaiSp");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSp,TenSp,Donvitinh,Dongia,MaLoaiSp,HinhSp")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiSp"] = new SelectList(_context.LoaiSps, "MaLoaiSp", "MaLoaiSp", sanPham.MaLoaiSp);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["MaLoaiSp"] = new SelectList(_context.LoaiSps, "MaLoaiSp", "MaLoaiSp", sanPham.MaLoaiSp);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSp,TenSp,Donvitinh,Dongia,MaLoaiSp,HinhSp")] SanPham sanPham)
        {
            if (id != sanPham.MaSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.MaSp))
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
            ViewData["MaLoaiSp"] = new SelectList(_context.LoaiSps, "MaLoaiSp", "MaLoaiSp", sanPham.MaLoaiSp);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaLoaiSpNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(string id)
        {
            return _context.SanPhams.Any(e => e.MaSp == id);
        }
    }
}
