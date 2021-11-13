using Pratica_Profissional.DAO;
using Pratica_Profissional.Models;
using Pratica_Profissional.Util;
using Pratica_Profissional.Util.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pratica_Profissional.Controllers
{
    public class ClienteController : Controller
    {
        DAOCliente daoCliente = new DAOCliente();
        //GET: Estados
        public ActionResult Index()
        {
            var daoCliente = new DAOCliente();
            List<Cliente> lista = daoCliente.GetClientes().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModel.ClienteVM model)
        {
            if (model.flTipo == "F")
            {
                if (string.IsNullOrEmpty(model.nmClienteFisico))
                {
                    ModelState.AddModelError("nmClienteFisico", "Por favor informe o cliente!");
                }

                if (model.nmClienteFisico != null)
                {
                    if (string.IsNullOrEmpty(model.nmClienteFisico.Trim()))
                    {
                        ModelState.AddModelError("nmClienteFisico", "Por favor informe o cliente!");
                    }
                }

                if (string.IsNullOrEmpty(model.dtNascimento))
                {
                    ModelState.AddModelError("dtNascimento", "Por favor informe a data de nascimento!");
                }

                if (string.IsNullOrEmpty(model.cpf))
                {
                    ModelState.AddModelError("cpf", "Por favor informe o CPF!");
                }
            }

            if (model.flTipo == "J")
            {
                if (string.IsNullOrEmpty(model.nmClienteJuridico))
                {
                    ModelState.AddModelError("nmClienteJuridico", "Por favor informe o cliente!");
                }

                if (model.nmClienteJuridico != null)
                {
                    if (string.IsNullOrEmpty(model.nmClienteJuridico.Trim()))
                    {
                        ModelState.AddModelError("nmClienteJuridico", "Por favor informe o cliente!");
                    }
                }

                if (string.IsNullOrEmpty(model.cnpj))
                {
                    ModelState.AddModelError("cnpj", "Por favor informe o CNPJ!");
                }

            }

            if (string.IsNullOrEmpty(model.cep))
            {
                ModelState.AddModelError("cep", "Por favor informe o CEP!");
            }

            if (string.IsNullOrEmpty(model.endereco))
            {
                ModelState.AddModelError("endereco", "Por favor informe o logradouro!");
            }

            if (string.IsNullOrEmpty(model.bairro))
            {
                ModelState.AddModelError("bairro", "Por favor informe o bairro!");
            }

            if (string.IsNullOrEmpty(model.nrEndereco))
            {
                ModelState.AddModelError("nrEndereco", "Por favor informe o número do endereço!");
            }

            if (string.IsNullOrEmpty(model.nrTel))
            {
                ModelState.AddModelError("nrTel", "Por favor informe o celular!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //Populando o objeto para salvar;
                    var dtAtual = DateTime.Today;
                    model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    var obj = model.VM2E(new Models.Cliente());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoCliente = new DAOCliente();

                    if (daoCliente.Create(obj))
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
        public ActionResult Edit(int id, ViewModel.ClienteVM model)
        {
            if (model.flTipo == "F")
            {
                if (string.IsNullOrEmpty(model.nmClienteFisico))
                {
                    ModelState.AddModelError("nmClienteFisico", "Por favor informe o cliente!");
                }

                if (model.nmClienteFisico != null)
                {
                    if (string.IsNullOrEmpty(model.nmClienteFisico.Trim()))
                    {
                        ModelState.AddModelError("nmClienteFisico", "Por favor informe o cliente!");
                    }
                }

                if (string.IsNullOrEmpty(model.dtNascimento))
                {
                    ModelState.AddModelError("dtNascimento", "Por favor informe a data de nascimento!");
                }

                if (string.IsNullOrEmpty(model.cpf))
                {
                    ModelState.AddModelError("cpf", "Por favor informe o CPF!");
                }
            }

            if (model.flTipo == "J")
            {
                if (string.IsNullOrEmpty(model.nmClienteJuridico))
                {
                    ModelState.AddModelError("nmClienteJuridico", "Por favor informe o cliente!");
                }

                if (model.nmClienteJuridico != null)
                {
                    if (string.IsNullOrEmpty(model.nmClienteJuridico.Trim()))
                    {
                        ModelState.AddModelError("nmClienteJuridico", "Por favor informe o cliente!");
                    }
                }

                if (string.IsNullOrEmpty(model.cnpj))
                {
                    ModelState.AddModelError("cnpj", "Por favor informe o CNPJ!");
                }

            }

            if (string.IsNullOrEmpty(model.cep))
            {
                ModelState.AddModelError("cep", "Por favor informe o CEP!");
            }

            if (string.IsNullOrEmpty(model.endereco))
            {
                ModelState.AddModelError("endereco", "Por favor informe o logradouro!");
            }

            if (string.IsNullOrEmpty(model.bairro))
            {
                ModelState.AddModelError("bairro", "Por favor informe o bairro!");
            }

            if (string.IsNullOrEmpty(model.nrEndereco))
            {
                ModelState.AddModelError("nrEndereco", "Por favor informe o número do endereço!");
            }

            if (string.IsNullOrEmpty(model.nrTel))
            {
                ModelState.AddModelError("nrTel", "Por favor informe o celular!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoCliente = new DAOCliente();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoCliente.GetClientesByID(id);
                    var obj = model.VM2E(bean);


                    if (daoCliente.Edit(obj))
                    {
                        TempData["message"] = "Registro alterado com sucesso!";
                        TempData["type"] = "sucesso";
                    }

                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["message"] = "O registro não foi alterado, ocorreram erros, verifique!";
                    TempData["type"] = "erro";
                    return View();
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
                var daoCliente = new DAOCliente();

                if (daoCliente.Delete(id))
                {
                    TempData["message"] = "Registro excluído com sucesso!";
                    TempData["type"] = "sucesso";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "O registro não pode ser excluído, pois está associado a uma cidade!";
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
                var daoCliente = new DAOCliente();

                var model = daoCliente.GetClientesByID(id);

                var VM = new ViewModel.ClienteVM
                {
                    idCliente = model.idPessoa,
                    nmClienteFisico = model.flTipo == "F" ? model.nmPessoa : null,
                    nmClienteJuridico = model.flTipo == "J" ? model.nmPessoa : null,
                    nmApelido = model.flTipo == "F" ? model.nmApelido : null,
                    nmFantasia = model.flTipo == "J" ? model.nmApelido : null,
                    rg = model.flTipo == "F" ? model.rg : null,
                    inscricaoEstadual = model.flTipo == "J" ? model.rg : null,
                    cpf = model.flTipo == "F" ? model.documento : null,
                    cnpj = model.flTipo == "J" ? model.documento : null,
                    genero = model.genero,
                    email = model.email,
                    endereco = model.endereco,
                    bairro = model.bairro,
                    nrEndereco = model.nrEndereco,
                    complemento = model.complemento,
                    cep = model.cep,
                    nrTel = model.nrTel,
                    dtNascimento = model.dtNascimento,
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy"),
                    condicaoPagamento = new ViewModel.CondicaoPagamentoVM
                    {
                        idCondicaoPagamento = model.condicaoPagamento.idCondicaoPagamento,
                        text = model.condicaoPagamento.nmCondicaoPagamento
                    },
                    cidade = new ViewModel.CidadeVM
                    {
                        idCidade = model.cidade.idCidade,
                        text = model.cidade.nmCidade
                    },
                    limiteCredito = model.limiteCredito,
                    dsObservacao = model.dsObservacao,
                    flTipo = model.flTipo,
                    dsTipo = model.flTipo == "F" ? "Físico" : "Jurídico",
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


        private IQueryable<dynamic> Find(int? id, string q)
        {
            var daoCliente = new DAOCliente();
            var list = daoCliente.GetClientes();
            var select = list.Select(u => new
            {
                id = u.idPessoa,
                text = u.nmPessoa,
                dtCadastro = u.dtCadastro,
                dtUltAlteracao = u.dtAtualizacao
            }).OrderBy(u => u.text).ToList();

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