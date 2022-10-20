﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using SharedModel.Models;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.Http.Json;
using CustomerSite.Extensions;
using System.Net.Http.Headers;

namespace CustomerSite.Controllers
{
    public class GioHangsController : Controller
    {
        private readonly qlbanhangContext _context;

        public GioHangsController(qlbanhangContext context)
        {
            _context = context;
        }

        // GET: GioHangs
        public async Task<IActionResult> Index()
        {
            var qlbanhangContext = _context.GioHangs.Include(g => g.MaKhNavigation).Include(g => g.MaSpNavigation).Where(g=>g.IsDatHang==false);
            return View(await qlbanhangContext.ToListAsync());
        }

        // GET: GioHangs/Details/5

        // GET: GioHangs/Create
        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh");
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp");
            return View();
        }

        // POST: GioHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<RedirectToActionResult> AddToCart(string masp)
        {
            if (_context.GioHangs.FirstOrDefault(m => m.MaSp == masp) == null)
            {
                GioHang gioHang = new GioHang();
                gioHang.MaSp = masp;
                gioHang.MaKh = "KH02";
                gioHang.IsDatHang = false;
                gioHang.SoLuong = 1;
                gioHang.ThanhTien = _context.SanPhams.Find(masp).Dongia * gioHang.SoLuong;
                var httpService = new HttpService(new HttpClient());
                var ghs = await httpService.PostAsync<GioHang>(url: "https://localhost:44327/api/GioHangs", data: gioHang);
            }
            else
            {
                GioHang gioHang = new GioHang();
                gioHang = _context.GioHangs.Where(m => m.MaSp == masp).Where(m=>m.IsDatHang==false).FirstOrDefault(m => m.MaKh == "KH02");
                if (gioHang != null)
                {
                    gioHang.SoLuong++;
                    var httpService = new HttpService(new HttpClient());
                    var ghs = await httpService.PutAsync<GioHang>(url: "https://localhost:44327/api/GioHangs", data: gioHang);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<RedirectToActionResult> Update(string MaSP, int txtSL)
        {
            GioHang gioHang = new GioHang();
            gioHang = _context.GioHangs.Where(m => m.MaSp == MaSP).Where(m => m.IsDatHang == false).FirstOrDefault(m => m.MaKh == "KH02");
            if (gioHang != null)
            {
                gioHang.SoLuong = txtSL;
                var httpService = new HttpService(new HttpClient());
                var ghs = await httpService.PutAsync<GioHang>(url: "https://localhost:44327/api/GioHangs", data: gioHang);
            }
            return RedirectToAction("Index");
        }
        public async Task<RedirectToActionResult> DelCartItem(string MaSP)
        {
            GioHang gioHang = new GioHang();
            gioHang = _context.GioHangs.Where(g => g.MaSp == MaSP).Where(g=>g.IsDatHang==false).FirstOrDefault(g => g.MaKh == "KH02");
            if (gioHang != null)
            {
                var httpService = new HttpService(new HttpClient());
                var ghs = await httpService.DeleteAsync<GioHang>(url: "https://localhost:44327/api/GioHangs", data: gioHang);
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Order(string MaKH)
        {
            List<GioHang> ghs = _context.GioHangs.Where(g => g.MaKh == MaKH).Where(g=>g.IsDatHang==false).ToList();
            foreach (GioHang gh in ghs)
            {
                if (gh != null)
                {
                    gh.IsDatHang = true;
                    var httpService = new HttpService(new HttpClient());
                    var put = await httpService.PutAsync<GioHang>(url: "https://localhost:44327/api/GioHangs", data: gh);
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
