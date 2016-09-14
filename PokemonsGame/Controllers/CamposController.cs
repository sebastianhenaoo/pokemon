
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PokemonsGame.Models;

namespace PokemonsGame.Controllers
{
    public class CamposController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Campos
        public ActionResult Index()
        {
            var campos = db.Campos.Include(c => c.EnemyPokemon).Include(c => c.MyPokemon);
            return View(campos.ToList());
        }

        // GET: Campos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campo campo = db.Campos.Find(id);
            if (campo == null)
            {
                return HttpNotFound();
            }
            return View(campo);
        }

        // GET: Campos/Create
        public ActionResult Create()
        {
            ViewBag.EnemyPokemonId = new SelectList(db.Pokemons, "Id", "Nombre");
            ViewBag.MyPokemonId = new SelectList(db.Pokemons, "Id", "Nombre");
            return View();
        }

        // POST: Campos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MyPokemonId,EnemyPokemonId")] Campo campo)
        {
            if (ModelState.IsValid)
            {
                db.Campos.Add(campo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EnemyPokemonId = new SelectList(db.Pokemons, "Id", "Nombre", campo.EnemyPokemonId);
            ViewBag.MyPokemonId = new SelectList(db.Pokemons, "Id", "Nombre", campo.MyPokemonId);
            return View(campo);
        }

        // GET: Campos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campo campo = db.Campos.Find(id);
            if (campo == null)
            {
                return HttpNotFound();
            }
            ViewBag.EnemyPokemonId = new SelectList(db.Pokemons, "Id", "Nombre", campo.EnemyPokemonId);
            ViewBag.MyPokemonId = new SelectList(db.Pokemons, "Id", "Nombre", campo.MyPokemonId);
            return View(campo);
        }

        // POST: Campos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MyPokemonId,EnemyPokemonId")] Campo campo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EnemyPokemonId = new SelectList(db.Pokemons, "Id", "Nombre", campo.EnemyPokemonId);
            ViewBag.MyPokemonId = new SelectList(db.Pokemons, "Id", "Nombre", campo.MyPokemonId);
            return View(campo);
        }

        // GET: Campos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campo campo = db.Campos.Find(id);
            if (campo == null)
            {
                return HttpNotFound();
            }
            return View(campo);
        }

        // POST: Campos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Campo campo = db.Campos.Find(id);
            db.Campos.Remove(campo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult MostrarPokemons()
        {
            var pokes = db.Pokemons.ToList();
            return PartialView("_MostrarPokemons", pokes);
        }
        public ActionResult AddPokemon(int? id)
        {
            for (int i = 1; i <= db.Pokemons.Count(); ++i)
            {
                Pokemon poke = db.Pokemons.Find(i);
                poke.Vida = 100;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Random rn = new Random();
            int n = db.Pokemons.Count();
            int a = rn.Next(1, n + 1);
            Pokemon pok = null;
            Pokemon enpok = null;
            Campo campo = null;
            while (a == id)
            {
                a = rn.Next(1, n + 1);
            }
            pok = db.Pokemons.Find(id);
            enpok = db.Pokemons.Find(a);

            campo = new Campo()
            {
                MyPokemon = pok,
                MyPokemonId = pok.Id,
                EnemyPokemon = enpok,
                EnemyPokemonId = enpok.Id
            };

            db.Campos.Add(campo);
            db.SaveChanges();
            return PartialView("_Fight", campo);
        }
        public ActionResult CheckMatch(int? id)
        {
            Campo campo = db.Campos.Find(id);
            Pokemon mypok = db.Pokemons.Find(campo.MyPokemonId);
            Pokemon enemypok = db.Pokemons.Find(campo.EnemyPokemonId);
            if ((enemypok.Vida <= 0) && (mypok.Vida) <= 0)
            {
                enemypok.Vida = 100;
                mypok.Vida = 100;
                db.SaveChanges();
                return PartialView("_Empate");
            }
            if ((enemypok.Vida <= 0) || (mypok.Vida <= 0))
            {
                if ((enemypok.Vida) <= 0)
                {
                    enemypok.Vida = 100;
                    mypok.Vida = 100;
                    Pokemon ganador = mypok;
                    db.SaveChanges();
                    return PartialView("_winner", ganador);

                }
                else if ((mypok.Vida) <= 0)
                {
                    enemypok.Vida = 100;
                    mypok.Vida = 100;
                    Pokemon ganador = enemypok;
                    db.SaveChanges();
                    return PartialView("_winner", ganador);
                }

            }
            return PartialView("_winner");

        }

        public JsonResult AtacarPokemon(int? id)
        {
            Campo campo = db.Campos.Find(id);
            Pokemon mypok = db.Pokemons.Find(campo.MyPokemonId);
            Pokemon enemypok = db.Pokemons.Find(campo.EnemyPokemonId);
            enemypok.Vida = enemypok.Vida - (mypok.Ataque-enemypok.Defensa);
            mypok.Vida = mypok.Vida - (enemypok.Ataque - mypok.Defensa);

            db.SaveChanges();
            return Json(new { jugador = mypok.Vida, enemigo = enemypok.Vida });
        }
        public JsonResult DefenderPokemon(int? id)
        {
            Campo campo = db.Campos.Find(id);
            Pokemon mypok = db.Pokemons.Find(campo.MyPokemonId);
            Pokemon enemypok = db.Pokemons.Find(campo.EnemyPokemonId);
            if (mypok.Vida < 50 && enemypok.Vida < 50)
            {
                mypok.Vida = mypok.Vida + 10;
                enemypok.Vida = enemypok.Vida + 8;
            }
            db.SaveChanges();
            return Json(new { jugador = mypok.Vida, enemigo = enemypok.Vida });
        }
        public ActionResult AtaqueEnemigo(int? id)
        {
            if (id == null)
            {

            }
            Campo campo = db.Campos.Find(id);
            Pokemon mypok = db.Pokemons.Find(campo.MyPokemon.Id);
            Pokemon enemypok = db.Pokemons.Find(campo.EnemyPokemon.Id);
            mypok.Vida = mypok.Vida - enemypok.Ataque;
            db.SaveChanges();
            return PartialView("_Fight", campo);
        }
    }
}
