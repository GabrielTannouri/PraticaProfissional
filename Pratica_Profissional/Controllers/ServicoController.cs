﻿using Pratica_Profissional.DAO;
using Pratica_Profissional.Models;
using Pratica_Profissional.Util;
using Pratica_Profissional.Util.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pratica_Profissional.Controllers
{
    public class ServicoController : Controller
    {
        DAOServico daoServico = new DAOServico();
        //GET: Estados
        public ActionResult Index()
        {
            var daoServico = new DAOServico();
            List<Servico> lista = daoServico.GetServicos().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ViewModel.ServicoVM model)
        {
            if (string.IsNullOrWhiteSpace(model.nmServico))
            {
                ModelState.AddModelError("nmServico", "Por favor informe o nome do serviço!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Setando as datas atuais;
                    var dtAtual = DateTime.Today;
                    model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.Servico());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoServico = new DAOServico();

                    if (daoServico.Create(obj))
                    {
                        TempData["message"] = "Registro inserido com sucesso!";
                        TempData["type"] = "sucesso";
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return (this.GetView(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, ViewModel.ServicoVM model)
        {
            if (string.IsNullOrWhiteSpace(model.nmServico))
            {
                ModelState.AddModelError("nmServico", "Por favor informe o nome do serviço!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoServico = new DAOServico();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoServico.GetServicosByID(id);
                    var obj = model.VM2E(bean);


                    if (daoServico.Edit(obj))
                    {
                        TempData["message"] = "Registro alterado com sucesso!";
                        TempData["type"] = "sucesso";
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return (this.GetView(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var daoServico = new DAOServico();

                if (daoServico.Delete(id))
                {
                    TempData["message"] = "Registro excluído com sucesso!";
                    TempData["type"] = "sucesso";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "O registro não pode ser excluído, verifique!";
                TempData["type"] = "erro";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return (this.GetView(id));
        }

        private ActionResult GetView(int id)
        {
            try
            {
                var daoServico = new DAOServico();

                var model = daoServico.GetServicosByID(id);

                var VM = new ViewModel.ServicoVM
                {
                    idServico = model.idServico,
                    nmServico = model.nmServico,
                    vlServico = model.vlServico,
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy"),
                };

                return View(VM);
            }
            catch
            {
                return View();
            }
        }

        public JsonResult JsDetails(int? id, string q)
        {
            try
            {
                var result = this.Find(id, q).FirstOrDefault();
                if (result != null)
                    return Json(result, JsonRequestBehavior.AllowGet);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }

        public JsonResult JsCreate(Servico servico)
        {
            var dtAtual = DateTime.Today;
            servico.dtCadastro = dtAtual;
            servico.dtAtualizacao = dtAtual;
            try
            {
                var daoServico = new DAOServico();
                daoServico.Create(servico);
                var result = new
                {
                    type = "success",
                    message = "Serviço adicionado",
                    model = servico
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JsUpdate(Servico model)
        {
            var daoServico = new DAOServico();
            daoServico.Edit(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro alterado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IQueryable<dynamic> Find(int? id, string q)
        {
            var daoServico = new DAOServico();
            var list = daoServico.GetServicos();
            var select = list.Select(u => new
            {
                id = u.idServico,
                text = u.nmServico,
                vlServico = u.vlServico,
                dtCadastro = u.dtCadastro,
                dtUltAlteracao = u.dtAtualizacao
            })
                .OrderBy(u => u.text)
                .ToList();

            if (id != null)
            {
                return select.Where(u => u.id == id).AsQueryable();
            }
            else
            {
                return select.AsQueryable();
            }
        }

        public JsonResult JsQuery([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            try
            {
                var select = this.Find(null, requestModel.Search.Value);

                var totalResult = select.Count();

                var result = select.OrderBy(requestModel.Columns, requestModel.Start, requestModel.Length).ToList();

                return Json(new DataTablesResponse(requestModel.Draw, result, totalResult, result.Count), JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }
    }
}