
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;
using System.Web.Services;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Xml;
using ColetaInjetoraToshiba.coleta;
using Opc.Da;
using System.Text;
using Newtonsoft.Json;
using System.Data.SqlClient;
using ColetaInjetoraToshiba.webservice;
using System.Windows.Forms;

namespace ColetaInjetoraToshiba.coleta
{
    class ColetaInjetoraHaitianListener
    {

        
        private bool isListenerContinuaExecutando = true;
        public msws.icDTO icdto_atual = null;
        public msws.icUpDTO icupdto_atual = null;

        private Logger log = LogManager.GetCurrentClassLogger();
        private Thread threadEnviarEvento;
        private Thread threadSelecionarOp;

        public System.Windows.Forms.TextBox textBox1;


        // Variaveis de configuracao Coleta

        public string opSelecionada = "";
        public string cdPtAtual = "";
        public string hostInjetora = "";
        
        //Variavéis Haitian
        Double temperatura = 0;
        Double TEMPERATURA_ZONA_1 = 0;
        Double TEMPERATURA_ZONA_2 = 0;
        Double TEMPERATURA_ZONA_3 = 0;
        Double TEMPERATURA_ZONA_4 = 0;
        Double TEMPERATURA_ZONA_5 = 0;
        Double TEMPERATURA_ZONA_6 = 0;
        Double TEMPERATURA_ZONA_7 = 0;

        // Pressão  
        Double PRESSAO_INJECAO_MAX_bar = 0;
        Double PRESSAO_INJECAO = 0;
        Double PRESSAO_INJECAO_2 = 0;
        Double PRESSAO_INJECAO_3 = 0;
        Double PRESSAO_INJECAO_4 = 0;
        Double PRESSAO_INJECAO_5 = 0;
        Double PRESSAO_INJECAO_6 = 0;
        Double PRESSAO_INJECAO_7 = 0;
        Double PRESSAO_FORCA_FECHAMENTO_bar = 0;
        Double PRESSAO_PROTECAO_MOLDE_bar = 0;
        Double PRESSAO_MOLDE_bar = 0;
        Double PRESSAO_RECALQUE_bar = 0;

        //Unidade Injeção 
        Double INJECAO_MOLDE_POSICAO_mm = 0;
        Double INJECAO_MOLDE_PRESSAO_bar = 0;
        Double INJECAO_MOLDE_FLUXO = 0;

        // Posição De injeção 
        Double POSICAO_INJECAO_1 = 0;
        Double POSICAO_INJECAO_2 = 0;
        Double POSICAO_INJECAO_3 = 0;
        Double POSICAO_INJECAO_4 = 0;
        Double POSICAO_INJECAO_5 = 0;

        // Tempos 
        Double TEMPO_INJECAO_REAL_s = 0;
        Double TEMPO_INJECAO_REAL_MAX_s = 0;
        Double TEMPO_RECALQUE_s = 0;
        Double TEMPO_ABERTURA_PLACA_MOVEL_s = 0;
        Double TEMPO_FECHAMENTO_PLACA_MOVEL_s = 0;
        Double TEMPO_EXTRACAO_s = 0;
        Double TEMPO_DOSAGEM_s = 0;
        Double TEMPO_RESFRIAMENTO_s = 0;
        Double TEMPO_PROTECAO_MOLDE_s = 0;

        int CONVERTER = 1000000;


        //Velocidade de Injeção
        Double VELOCIDADE_INJECAO_1 = 0;
        Double VELOCIDADE_INJECAO_2 = 0;
        Double VELOCIDADE_INJECAO_3 = 0;
        Double VELOCIDADE_INJECAO_4 = 0;
        Double VELOCIDADE_INJECAO_5 = 0;
        Double VELOCIDADE_INJECAO_6 = 0;
        
        TextBox valor;
        TextBox parametro;
        TextBox maquina;
        TextBox Op;
        TextBox Mac;
        TextBox Conexao;

        //Perfil Contra Pressão
        Double POSICAO_RECALQUE_mm = 0;
        Double CURSO_DOSAGEM_mm = 0;
        Double ROTACAO_POR_MINUTO_ROSCA_rpm = 0;
        Double CONTRAPRESSAO_MAX_bar = 0;

        //Unidade de Fechamento
        Double MOLDE_FECHADO_PRESSAO_1_bar = 0;
        Double MOLDE_FECHADO_VELOCIDADE_1 = 0;
        Double MOLDE_FECHADO_FECHAR_MOLDE_POSICAO_1_mm = 0;
        Double MOLDE_FECHADO_PRESSAO_2_bar = 0;
        Double MOLDE_FECHADO_VELOCIDADE_2 = 0;
        Double MOLDE_FECHADO_FECHAR_MOLDE_POSICAO_2_mm = 0;
        Double MOLDE_FECHADO_PRESSAO_bar = 0;
        Double MOLDE_FECHADO_VELOCIDADE_3 = 0;
        Double MOLDE_FECHADO_FECHAR_MOLDE_POSICAO_3_mm = 0;
        Double MOLDE_FECHADO_TEMPO_PROTECAO_MOLDE_s = 0;
        Double MOLDE_FECHADO_TEMPO_RETARDO_FECHAMENTO_s = 0;
        Double MOLDE_FECHADO_TEMPO_RETARDO_ABERTURA_s = 0;


        //Unidade de abertura do molde
        Double ABERTURA_MOLDE_POSICAO_mm = 0;
        Double ABERTURA_MOLDE_PRESSAO_bar = 0;
        Double ABERTURA_MOLDE_FLUXO = 0;

        

        //Unidade Recalque
        Double RECALQUE_TEMPO_s = 0;
        Double RECALQUE_PRESSAO_bar = 0;
        Double RECALQUE_FLUXO = 0;

        //Unidade Dosagem
        Double DOSAGEM_POSICAO_mm = 0;
        Double DOSAGEM_PRESSAO_bar = 0;
        Double DOSAGEM_FLUXO = 0;
        Double DOSAGEM_CONTRA_PRESSAO_bar = 0;


        //Extração
        Double EXTRACAO_POSICAO_mm = 0;
        Double EXTRACAO_RECUO_EXTRATOR_mm = 0;
        Double EXTRACAO_POSICAO_1_EXTRATOR_PARA_FRENTE_mm = 0;
        Double EXTRACAO_PRESSAO_1_EXTRATOR_PARA_FRENTE_bar = 0;
        Double EXTRACAO_VELOCIDADE_1_EXTRATOR_PARA_FRENTE = 0;
        Double EXTRACAO_POSICAO_2_EXTRATOR_PARA_FRENTE_mm = 0;
        Double EXTRACAO_PRESSAO_2_EXTRATOR_PARA_FRENTE_bar = 0;
        Double EXTRACAO_VELOCIDADE_2_EXTRATOR_PARA_FRENTE = 0;
        Double EXTRACAO_ATRASO_EXTRACAO_s = 0;
        Double EXTRACAO_ATRASO_EXTRACAO_NO_FINAL_s = 0;
        Double EXTRACAO_POSICAO_1_ATRASO_EXTRACAO_mm = 0;
        Double EXTRACAO_PRESSAO_1_ATRASO_EXTRACAO_bar = 0;
        Double EXTRACAO_VELOCIDADE_1_ATRASO_EXTRACAO = 0;
        Double EXTRACAO_PRESSAO_2_ATRASO_EXTRACAO_bar = 0;
        Double EXTRACAO_VELOCIDADE_2_ATRASO_EXTRACAO = 0;
        Double EXTRACAO_RETARDO_NA_EXTRACAO_s = 0;
        Double EXTRACAO_TEMPO_MOVIMENTACAO_EXTRAÇÃO_s = 0;


        String ADRESS_PRESSAO_INJECAO_bar = ".SVs.system.sv_PreInjectVis.Points[1].press";
       

     
    //Equipamentos auxiliares

    //Aquecedor de molde 
    Double Temperatura_Controle_Aquecedor_Molde = 0;
    Double Temperatura_Agua_Retorno_Aquecedor_Molde =0;
    Double Temperatura_Agua_Saida_Aquecedor_Molde=0;
    Double Temperatura_Configurada_Aquecedor_Molde=0;
    Double Temperatura_Atual_Aquecedor_Molde = 0;


    //Chillers (Geladeira) 

    Double Temperatura_Controle_Chiller =0;
    Double Temperatura_Configurada_Chiller=0;
    Double Temperatura_Atual_Chillers=0;

    //Dryer(Desumidificador) 
    Double Temperatura_Configurada_Desumidificador=0;
    Double Temperatura_Atual_Desumidificador=0;
    Double Temperatura_Controle_Desumidificador = 0;

        // Endereços OPC Haitian
        string item_alarme_Pendente_adress = ".SVs.system.sv_PendingAlarms";
        string item_alarme_read_adress = ".Funcs.Alarm.ReadAlarms().Out.Alarms";
        string item_temperatura_1_adress = ".SVs.system.sv_TempZone1";
        string item_temperatura_2_adress = ".SVs.system.sv_TempZone2";
        string item_temperatura_3_adress = ".SVs.system.sv_TempZone3";
        string item_temperatura_4_adress = ".SVs.system.sv_TempZone4";
        string item_temperatura_5_adress = ".SVs.system.sv_TempZone5";
        string item_temperatura_6_adress = ".SVs.system.sv_TempZone6";
        string item_pressao_injecao_max_adress = ".SVs.system.sv_PreInjectVis.Points[1].press";
        string item_pressao_injecao_adress = ".SVs.system.sv_PreInjectVis.Points[1].press";
        string item_pressao_molde_adress = ".SVs.system.sv_PreInjectVis.Points[1].press";
        string item_pressao_recalque_adress = ".SVs.system.sv_PostInjectVis.Points[1].press";
        string item_PRESSAO_FORCA_FECHAMENTO_bar_adress = ".SVs.system.sv_MoldFwdVis.Points[1].press";
        string item_PRESSAO_PROTECAO_MOLDE_bar_adress = ".SVs.system.sv_MoldFwdVis.Points[4].press";
        string item_TEMPO_INJECAO_REAL_s_adress = ".SVs.system.sv_InjectTime";
        string item_TEMPO_RECALQUE_s_adress = ".SVs.system.sv_InjectHoldTime";
        string item_TEMPO_ABERTURA_PLACA_MOVEL_s_adress = ".SVs.system.sv_MoldOpenTime";
        string item_TEMPO_FECHAMENTO_PLACA_MOVEL_s_adress = ".SVs.system.sv_MoldCloseTime";
        string item_TEMPO_EXTRACAO_s_adress = ".SVs.system.sv_EjectorMoveTime";
        string item_TEMPO_DOSAGEM_s_adress = ".SVs.system.sv_ChargeTime";
        string item_TEMPO_RESFRIAMENTO_s_adress = ".SVs.system.sv_CoolingTime";
        string item_TEMPO_PROTECAO_MOLDE_s_adress = ".SVs.system.sv_MoldProtectionTime";
        string item_VELOCIDADE_INJECAO_1_adress = ".SVs.system.sv_PreInjectVis.Points[1].speed";
        string item_VELOCIDADE_INJECAO_2_adress = ".SVs.system.sv_PreInjectVis.Points[2].speed";
        string item_VELOCIDADE_INJECAO_3_adress = ".SVs.system.sv_PreInjectVis.Points[3].speed";
        string item_VELOCIDADE_INJECAO_4_adress = ".SVs.system.sv_PreInjectVis.Points[4].speed";
        string item_VELOCIDADE_INJECAO_5_adress = ".SVs.system.sv_PreInjectVis.Points[5].speed";
        string item_VELOCIDADE_INJECAO_6_adress = ".SVs.system.sv_PreInjectVis.Points[6].speed";
        string item_POSICAO_RECALQUE_mm_adress = ".SVs.system.sv_PostInjectVis.Points[2].startPos";
        string item_CURSO_DOSAGEM_mm_adress = ".SVs.system.sv_rScrewCurrentPosVis";
        string item_CONTRAPRESSAO_MAX_bar_adress = ".SVs.system.sv_ConstOutputBPPlastBefInj1.OutputValue";
        string item_MOLDE_FECHADO_PRESSAO_1_bar_adress = ".SVs.system.sv_MoldFwdVis.Points[1].press";
        string item_MOLDE_FECHADO_VELOCIDADE_1_adress = ".SVs.system.sv_MoldFwdVis.Points[1].speed";
        string item_MOLDE_FECHADO_FECHAR_MOLDE_POSICAO_1_mm_adress = ".SVs.system.sv_MoldFwdVis.Points[1].startPos";
        string item_ABERTURA_MOLDE_POSICAO_mm_adress = ".SVs.system.sv_MoldBwdVis.Points[6].startPos";
        string item_DOSAGEM_CONTRA_PRESSAO_bar_adress =  ".SVs.system.sv_ConstOutputBPPlastBefInj1.OutputValue";
        string item_DOSAGEM_FLUXO_adress =  ".SVs.system.sv_AutoPurgeParams.PlasticizingParam.ConstOutputVel.OutputValue";
        string item_INJECAO_MOLDE_POSICAO_mm_adress = ".SVs.system.sv_PreInjectVis.Points[2].startPos";
        string item_ABERTURA_MOLDE_FLUXO_adress = ".SVs.system.sv_MoldBwdVis.Points[1].speed";
        string item_ABERTURA_MOLDE_PRESSAO_bar_adress =  ".SVs.system.sv_MoldBwdVis.Points[1].press";
        string item_INJECAO_MOLDE_PRESSAO_bar_adress =  ".SVs.system.sv_PreInjectVis.Points[1].press";
        string item_INJECAO_MOLDE_FLUXO_adress = ".SVs.system.sv_PreInjectVis.Points[1].speed";
        string item_RECALQUE_PRESSAO_bar_adress = ".SVs.system.sv_PostInjectVis.Points[1].press";
        string item_RECALQUE_FLUXO_adress = ".SVs.system.sv_PostInjectVis.Points[1].speed";
        string item_RECALQUE_TEMPO_s_adress =  ".SVs.system.sv_InjectHoldTime";
        string item_DOSAGEM_POSICAO_mm_adress = ".SVs.system.sv_rScrewCurrentPosVis";
        string item_DOSAGEM_PRESSAO_bar_adress =".SVs.system.sv_AutoPurgeParams.PlasticizingParam.ConstOutputPre.OutputValue";
        string item_EXTRACAO_POSICAO_mm_adress =  ".SVs.system.sv_rEjectCurrentPosVis";


        System.Random random = new System.Random();
        msws.icDTO icdto = null;
        msws.icUpDTO icupdto = null;


        //Banco de Dados Injet
        String strcon = "Data Source=MAPTECH64;Initial Catalog=INJET;Persist Security Info=True;User ID=sa;Password=sa123";

        #region OPC private fields

        private Server serverAtual;
        private OpcCom.Factory fact = new OpcCom.Factory();

        private Subscription groupRead;
        private SubscriptionState groupState;

        private Subscription groupWrite;
        private SubscriptionState groupStateWrite;

        private List<Item> itemsList = new List<Item>();

        String equimentoAuxiliar;

        #endregion

        #region Inicializar Coleta
        public void inicializaColeta(string cdPt, string opcHost, msws.icDTO icdto, msws.icUpDTO icpudto,Server server)
        {
            log.Info("Iniciando Threads");

            try
            {
                isListenerContinuaExecutando = true;
                cdPtAtual = cdPt;
                icdto_atual = icdto;
                icupdto_atual = icpudto;
                hostInjetora = opcHost;

                //Mac = Application.OpenForms["TelaPrincipal"].Controls["textBoxMac"] as TextBox;
               // Mac.Text = "Iniciando";
                serverAtual = server;
                ConnectToOpcServer(hostInjetora, serverAtual);


                equimentoAuxiliar = ColetaInjetoraToshiba.Properties.Settings.Default.EquipamentoAuxiliar;
                threadSelecionarOp = new Thread(OpListening);
                threadSelecionarOp.Name = cdPtAtual;
                threadSelecionarOp.Start();
           
                threadEnviarEvento = new Thread(enviarEventoThread);
                threadEnviarEvento.Name = "threadEnviarEvento";
                threadEnviarEvento.Start();
   
            }
            catch (Exception e)
            {
                log.Error("Ocorreu o seguinte erro no metodo inicializaThreads() - " + e.Message);
            }
        }
        #endregion

        #region Op Thread
        public void OpListening()
        {

            while (isListenerContinuaExecutando == true)
            {

                try
                {
                    opSelecionada = getOpSelecionada(cdPtAtual);
  
                    Console.WriteLine("A op selecionada : " +cdPtAtual+": "+ hostInjetora+": "+ opSelecionada);
                    Thread.Sleep(60000);
                }
                catch (Exception ex)
                {
                }

           }

        }
        #endregion
        private string getOpSelecionada(string cdPt)
        {
            string opselecionadateste = "";

            try
            {

                OpRequest op = new OpRequest();

                op.dtReferencia = DateTime.Now.ToString("dd/MM/yyyy");
                op.cdPosto = cdPt;
                op.idTurno = TurnoRequest();
                op.filtroOp = 0;
                op.tpId = 1;
                op.cdCp = "";

                string json = JsonConvert.SerializeObject(op);
                // Create a request using a URL that can receive a post.   
                WebRequest request = WebRequest.Create("http://localhost:8080/idw/rest/injet/monitorizacao/detalhe");
                // Set the Method property of the request to POST.  
                request.Method = "POST";

                // Create POST data and convert it to a byte array.  
                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/json; charset=utf-8";
                // Set the ContentLength property of the WebRequest.  
                request.ContentLength = byteArray.Length;

                // Get the request stream.  
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStream.Close();

                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.  
                // The using block ensures the stream is automatically closed.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.  
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.  
                    string responseFromServer = reader.ReadToEnd();
                    //var post = JsonConvert.DeserializeObject<RootObject>(responseFromServer.ToString());
                    dynamic post= Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer.ToString());
                    // Display the content.  
                    Console.WriteLine(post.opSelecionada);
                    opselecionadateste = post.opSelecionada;
                }
                // Close the response.  
                response.Close();
            }
            catch (Exception exc)
            {

            }
            return opselecionadateste;

        }

        private int TurnoRequest()
        {
            var httpClient = new HttpClient();
            int idTurno = 0;
            var Response = httpClient.GetStringAsync("http://localhost:8080/idw/rest/injet/monitorizacao/turnoAtual");
            var Result = Response.Result;

            var requisicaoWeb = WebRequest.Create("http://localhost:8080/idw/rest/injet/monitorizacao/turnoAtual");
            requisicaoWeb.Method = "GET";

            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                var post = JsonConvert.DeserializeObject<Turno>(objResponse.ToString());
                //Console.WriteLine(post.idTurno + " " + post.dsTurno + " ");
                streamDados.Close();
                resposta.Close();
                idTurno = post.idTurno;
            }

            return idTurno;
        }

        #region enviarEventoThread
        private void enviarEventoThread()
        {

            while (isListenerContinuaExecutando == true)
            {
                if (icupdto_atual != null)
                {

                    // Intervalo entre amostras enviadas
                    int DELAY = 10000;


                    // Enviandos dados de Temperatura de Injeção por Zonas

                    try
                    {

                        //Temperatura 1 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, temperatura, 5.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        //Temperatura 2 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPERATURA_ZONA_2, 111.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        //Temperatura 3 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPERATURA_ZONA_3, 112.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        //Temperatura 4 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPERATURA_ZONA_4, 113.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        //Temperatura 5 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPERATURA_ZONA_5, 114.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        //Temperatura 6
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPERATURA_ZONA_6, 115.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        //Temperatura 7
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPERATURA_ZONA_7, 116.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    // Enviando os eventos de Pressão de Injeção

                    try
                    {
                        // Pressão de injeção 1
                        Console.Out.WriteLine(isListenerContinuaExecutando);
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, PRESSAO_INJECAO, 23.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Pressão de injeção 2
                        Console.Out.WriteLine(isListenerContinuaExecutando);
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, PRESSAO_INJECAO_2, 118.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Pressão de injeção 3
                        Console.Out.WriteLine(isListenerContinuaExecutando);
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, PRESSAO_INJECAO_3, 119.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Pressão de injeção 4
                        Console.Out.WriteLine(isListenerContinuaExecutando);
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, PRESSAO_INJECAO_4, 120.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Pressão de injeção 5
                        Console.Out.WriteLine(isListenerContinuaExecutando);
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, PRESSAO_INJECAO_5, 121.0, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);


                    // Velocidade de Injeção 

                    try
                    {
                        // Velocidade de Injeção 1
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, VELOCIDADE_INJECAO_1, 55, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    } Thread.Sleep(DELAY);

                    try
                    {
                        // Velocidade de Injeção 2
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, VELOCIDADE_INJECAO_2, 56, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);
                    try
                    {
                        // Velocidade de Injeção 3
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, VELOCIDADE_INJECAO_3, 57, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);
                    try
                    {
                        // Velocidade de Injeção 4
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, VELOCIDADE_INJECAO_4, 58, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);
                    try
                    {
                        // Velocidade de Injeção 5
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, VELOCIDADE_INJECAO_5, 59, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Velocidade de Injeção 6 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, VELOCIDADE_INJECAO_6, 60, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);


                    // Eventos posição de Injeção

                    try
                    {
                        // Velocidade de Injeção 6 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, VELOCIDADE_INJECAO_6, 60, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }


                    // Eventos de posição de Injeção

                    try
                    {
                        // Eventos de posição de Injeção 1
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, POSICAO_INJECAO_1, 34, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);


                    try
                    {
                        // Eventos de posição de Injeção 2
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, POSICAO_INJECAO_2, 118, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Eventos de posição de Injeção 3
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, POSICAO_INJECAO_3, 119, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Eventos de posição de Injeção 4
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, POSICAO_INJECAO_4, 120, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Eventos de posição de Injeção 5
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, POSICAO_INJECAO_5, 121, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);
                    

                    // Enviando os enventos de tempo
                   
                    try
                    {
                        // Tempo de Injeção
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPO_INJECAO_REAL_s, 47.0, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Tempo de Recalque
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPO_RECALQUE_s, 48.0, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);
                    try
                    {
                        // Tempo de Dosagem 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPO_DOSAGEM_s, 52.0, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                     try
                    {
                        // Tempo de Resfriamento
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPO_RESFRIAMENTO_s, 53.0, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);
                    
                     try
                    {
                        // Tempo de Proteção do Molde
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPO_PROTECAO_MOLDE_s, 54.0, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        //Tempo de fechamento do o Molde 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPO_FECHAMENTO_PLACA_MOVEL_s, 50, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        // Tempo abertura Molde
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPO_ABERTURA_PLACA_MOVEL_s, 49, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);

                    try
                    {
                        //Tempo de extração
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, TEMPO_EXTRACAO_s, 51, icupdto_atual, icdto_atual, opSelecionada);
                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }
                    Thread.Sleep(DELAY);


                    
                    // Equipamentos Auxiliares

                    if(String.Compare(hostInjetora,equimentoAuxiliar)==0){
                        try
                        {
                            // Temperatura de Controle Desumidificador
                            MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, Temperatura_Controle_Desumidificador, 137, icupdto_atual, icdto_atual, opSelecionada);
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(30000);
                        }
                        Thread.Sleep(DELAY);

                        try
                        {

                            // Temperatura Controle do Chiller/Geladeira
                            MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, Temperatura_Controle_Chiller, 132, icupdto_atual, icdto_atual, opSelecionada);
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(30000);
                        }
                        Thread.Sleep(DELAY);
                        try
                        {

                            //Temperatura Atual Chiller
                            MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, Temperatura_Atual_Chillers, 134, icupdto_atual, icdto_atual, opSelecionada);
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(30000);
                        }
                        Thread.Sleep(DELAY);

                        try
                        {
                            //Temperatura Agua de Saida Aquecedor de molde
                            MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, Temperatura_Agua_Saida_Aquecedor_Molde, 129, icupdto_atual, icdto_atual, opSelecionada);
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(30000);
                        }
                        Thread.Sleep(DELAY);
                        try
                        {
                            //Temperatura Agua de retorno Aquecedor do Molde
                            MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, Temperatura_Agua_Retorno_Aquecedor_Molde, 128, icupdto_atual, icdto_atual, opSelecionada);
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(30000);
                        }
                        Thread.Sleep(DELAY);
                        try
                        {
                            // Temperatura de controle Aquecedor de Molde
                            MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, Temperatura_Controle_Aquecedor_Molde, 127, icupdto_atual, icdto_atual, opSelecionada);
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(30000);
                        }
                        Thread.Sleep(DELAY);



                    }
                    

                    // Eventos de Dosagem
                    try
                    {
                        // Pressão de Dosagem
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, DOSAGEM_PRESSAO_bar, 41, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }

                    Thread.Sleep(DELAY);

                    try
                    {
                        // Dosagem posição
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, DOSAGEM_POSICAO_mm, 40, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }

                    Thread.Sleep(DELAY);

                    try
                    {
                        // Dosagem Contra Dosagem 
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, DOSAGEM_CONTRA_PRESSAO_bar, 43, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }

                    Thread.Sleep(DELAY);

                    // Perfil de Recalque

                    try
                    {
                        // Pressão de Recalque
                        MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, PRESSAO_RECALQUE_bar, 37, icupdto_atual, icdto_atual, opSelecionada);

                    }
                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }

                    Thread.Sleep(DELAY);

                    ////Perfil de abertura e fechamento do Molde

                    //try
                    //{
                    //    MswsStubdelegate.getInstancia().getMsWs().setCepOPC(0, ABERTURA_MOLDE_POSICAO_mm, 31, icupdto_atual, icdto_atual, opSelecionada);

                    //}
                    //catch (Exception ex)
                    //{
                    //    log.Info("Erro na chamada do webservice: " + ex);
                    //    Thread.Sleep(30000);
                    //}

                    //Thread.Sleep(DELAY);
                    //Console.Out.WriteLine("Aguardando 1 MINUTO");


                }
            }

        }
        #endregion


        private void ConnectToOpcServer(string hostatual,Server serveratual)
        {
            // 1st: Create a server object and connect to the RSLinx OPC Server
            try
            {
                //serveratual= new Opc.Da.Server(fact, null);
                ////server.Url = new Opc.URL("opcda://Sigmatek.OPCServer.1");

                //serveratual.Url = new Opc.URL("opcda://KeStudio.Opc.LC.1.96");
                ////hostatual = "teste";
                ////2nd: Connect to the created server
                //serveratual.Connect();
                
                //Read group subscription
                groupState = new Opc.Da.SubscriptionState();
                groupState.Name = hostatual;

                groupState.UpdateRate = 600;
                groupState.Active = true;

                //Read group creation
                groupRead = (Opc.Da.Subscription)serveratual.CreateSubscription(groupState);
                groupRead.DataChanged += new Opc.Da.DataChangedEventHandler(groupRead_DataChanged);
                

                //Parametros de Leitura

                // Verificar Alertas Pendentes  na Injetora
                //Item item_alarme_Pendente = new Item();
                //item_alarme_Pendente.ItemName = hostatual + ".SVs.system.sv_PendingAlarms";
                //itemsList.Add(item_alarme_Pendente);

                ////Leitura dos Alarmes da  Injetora
                //Item item_alarme = new Item();
                //item_alarme.ItemName = hostatual + ".Funcs.Alarm.ReadAlarms().Out.Alarms";
                //itemsList.Add(item_alarme);



                // Temperatura 1
                Item item_temperatura_1 = new Item();
                item_temperatura_1.ItemName = hostatual + ".SVs.system.sv_TempZone1";
                itemsList.Add(item_temperatura_1);

                // Temperatura 2
                Item item_temperatura_2 = new Item();
                item_temperatura_2.ItemName = hostatual + ".SVs.system.sv_TempZone2";
                itemsList.Add(item_temperatura_2);


                // Temperatura 3
                Item item_temperatura_3 = new Item();
                item_temperatura_3.ItemName = hostatual + ".SVs.system.sv_TempZone3";
                itemsList.Add(item_temperatura_3);

                // Temperatura 4
                Item item_temperatura_4 = new Item();
                item_temperatura_4.ItemName = hostatual + ".SVs.system.sv_TempZone4";
                itemsList.Add(item_temperatura_4);

                // Temperatura 5
                Item item_temperatura_5 = new Item();
                item_temperatura_5.ItemName = hostatual + ".SVs.system.sv_TempZone5";
                itemsList.Add(item_temperatura_5);

                // Temperatura 6
                Item item_temperatura_6 = new Item();
                item_temperatura_6.ItemName = hostatual + ".SVs.system.sv_TempZone6";
                itemsList.Add(item_temperatura_6);

                // Temperatura Zona 7
                Item item_temperatura_7= new Item();
                item_temperatura_7.ItemName = hostatual + ".SVs.system.sv_TempZone7";
                itemsList.Add(item_temperatura_7);


                //Pressão de Injeção  Maxima
                Item item_pressao_injecao_max = new Item();
                item_pressao_injecao_max.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[1].press";
                itemsList.Add(item_pressao_injecao_max);

                //Pressão de Injeção  
                Item item_pressao_injecao = new Item();
                item_pressao_injecao.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[1].press";
                itemsList.Add(item_pressao_injecao);

                //Pressão de Injeção   2
                Item item_pressao_injecao_2 = new Item();
                item_pressao_injecao_2.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[2].press";
                itemsList.Add(item_pressao_injecao_2);

                //Pressão de Injeção 3
                Item item_pressao_injecao_3 = new Item();
                item_pressao_injecao_3.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[3].press";
                itemsList.Add(item_pressao_injecao_3);

                //Pressão de Injeção 4
                Item item_pressao_injecao_4 = new Item();
                item_pressao_injecao_4.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[4].press";
                itemsList.Add(item_pressao_injecao_4);

                //Pressão de Injeção 5
                Item item_pressao_injecao_5 = new Item();
                item_pressao_injecao_5.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[5].press";
                itemsList.Add(item_pressao_injecao_5);

                //Pressão de Injeção 6
                Item item_pressao_injecao_6 = new Item();
                item_pressao_injecao_6.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[6].press";
                itemsList.Add(item_pressao_injecao_6);

                //Pressão Molde
                Item item_pressao_molde = new Item();
                item_pressao_molde.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[1].press";
                itemsList.Add(item_pressao_molde);

                //Pressão Recalque
                Item item_pressao_recalque = new Item();
                item_pressao_recalque.ItemName = hostatual + ".SVs.system.sv_PostInjectVis.Points[1].press";
                itemsList.Add(item_pressao_recalque);


                //PRESSAO_FORCA_FECHAMENTO_bar

                Item item_PRESSAO_FORCA_FECHAMENTO_bar = new Item();
                item_PRESSAO_FORCA_FECHAMENTO_bar.ItemName = hostatual + ".SVs.system.sv_MoldFwdVis.Points[1].press";
                itemsList.Add(item_PRESSAO_FORCA_FECHAMENTO_bar);

                //PRESSAO_PROTECAO_MOLDE_bar(46)

                Item item_PRESSAO_PROTECAO_MOLDE_bar = new Item();
                item_PRESSAO_PROTECAO_MOLDE_bar.ItemName = hostatual + ".SVs.system.sv_MoldFwdVis.Points[4].press";
                itemsList.Add(item_PRESSAO_PROTECAO_MOLDE_bar);


                // TEMPO_INJECAO_REAL_s
                Item item_TEMPO_INJECAO_REAL_s = new Item();
                item_TEMPO_INJECAO_REAL_s.ItemName = hostatual + ".SVs.system.sv_InjectTime";
                itemsList.Add(item_TEMPO_INJECAO_REAL_s);


                //TEMPO_RECALQUE_s
                Item item_TEMPO_RECALQUE_s = new Item();
                item_TEMPO_RECALQUE_s.ItemName = hostatual + ".SVs.system.sv_InjectHoldTime";
                itemsList.Add(item_TEMPO_RECALQUE_s);

                //TEMPO_ABERTURA_PLACA_MOVEL_s
                Item item_TEMPO_ABERTURA_PLACA_MOVEL_s = new Item();
                item_TEMPO_ABERTURA_PLACA_MOVEL_s.ItemName = hostatual + ".SVs.system.sv_MoldOpenTime";
                itemsList.Add(item_TEMPO_ABERTURA_PLACA_MOVEL_s);

                //item_TEMPO_FECHAMENTO_PLACA_MOVEL_s
                Item item_TEMPO_FECHAMENTO_PLACA_MOVEL_s = new Item();
                item_TEMPO_FECHAMENTO_PLACA_MOVEL_s.ItemName = hostatual + ".SVs.system.sv_MoldCloseTime";
                itemsList.Add(item_TEMPO_FECHAMENTO_PLACA_MOVEL_s);

                //TEMPO_EXTRACAO_s
                Item item_TEMPO_EXTRACAO_s = new Item();
                item_TEMPO_EXTRACAO_s.ItemName = hostatual + ".SVs.system.sv_EjectorMoveTime";
                itemsList.Add(item_TEMPO_EXTRACAO_s);

                //TEMPO_DOSAGEM_s
                Item item_TEMPO_DOSAGEM_s = new Item();
                item_TEMPO_DOSAGEM_s.ItemName = hostatual + ".SVs.system.sv_ChargeTime";
                itemsList.Add(item_TEMPO_DOSAGEM_s);

                //TEMPO_RESFRIAMENTO_s

                Item item_TEMPO_RESFRIAMENTO_s = new Item();
                item_TEMPO_RESFRIAMENTO_s.ItemName = hostatual + ".SVs.system.sv_CoolingTime";
                itemsList.Add(item_TEMPO_RESFRIAMENTO_s);

                
                //TEMPO_PROTECAO_MOLDE_s

                Item item_TEMPO_PROTECAO_MOLDE_s = new Item();
                item_TEMPO_PROTECAO_MOLDE_s.ItemName = hostatual + ".SVs.system.sv_MoldProtectionTime";
                itemsList.Add(item_TEMPO_PROTECAO_MOLDE_s);

                //VELOCIDADE_INJECAO_1
                Item item_VELOCIDADE_INJECAO_1 = new Item();
                item_VELOCIDADE_INJECAO_1.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[1].speed";
                itemsList.Add(item_VELOCIDADE_INJECAO_1);

                //VELOCIDADE_INJECAO_2
                Item item_VELOCIDADE_INJECAO_2 = new Item();
                item_VELOCIDADE_INJECAO_2.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[2].speed";
                itemsList.Add(item_VELOCIDADE_INJECAO_2);

                //VELOCIDADE_INJECAO_3
                Item item_VELOCIDADE_INJECAO_3 = new Item();
                item_VELOCIDADE_INJECAO_3.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[3].speed";
                itemsList.Add(item_VELOCIDADE_INJECAO_3);

                //VELOCIDADE_INJECAO_4
                Item item_VELOCIDADE_INJECAO_4 = new Item();
                item_VELOCIDADE_INJECAO_4.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[4].speed";
                itemsList.Add(item_VELOCIDADE_INJECAO_4);

                //VELOCIDADE_INJECAO_5
                Item item_VELOCIDADE_INJECAO_5 = new Item();
                item_VELOCIDADE_INJECAO_5.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[5].speed";
                itemsList.Add(item_VELOCIDADE_INJECAO_5);

                //VELOCIDADE_INJECAO_6
                Item item_VELOCIDADE_INJECAO_6 = new Item();
                item_VELOCIDADE_INJECAO_6.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[6].speed";
                itemsList.Add(item_VELOCIDADE_INJECAO_6);

                //INJECAO_MOLDE_POSICAO_mm
                Item item_INJECAO_MOLDE_POSICAO_mm = new Item();
                item_INJECAO_MOLDE_POSICAO_mm.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[1].startPos";
                itemsList.Add(item_INJECAO_MOLDE_POSICAO_mm);

                //POSIÇÃO INJEÇÃO 2
                Item item_POSICAO_INJECAO_2= new Item();
                item_POSICAO_INJECAO_2.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[2].startPos";
                itemsList.Add(item_POSICAO_INJECAO_2);

                //POSIÇÃO INJEÇÃO 3
                Item item_POSICAO_INJECAO_3 = new Item();
                item_POSICAO_INJECAO_3.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[3].startPos";
                itemsList.Add(item_POSICAO_INJECAO_3);

                //POSIÇÃO INJEÇÃO 4
                Item item_POSICAO_INJECAO_4 = new Item();
                item_POSICAO_INJECAO_4.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[4].startPos";
                itemsList.Add(item_POSICAO_INJECAO_4);

                //POSIÇÃO INJEÇÃO 5
                Item item_POSICAO_INJECAO_5 = new Item();
                item_POSICAO_INJECAO_5.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[5].startPos";
                itemsList.Add(item_POSICAO_INJECAO_5);

                //POSICAO_RECALQUE_mm
                Item item_POSICAO_RECALQUE_mm = new Item();
                item_POSICAO_RECALQUE_mm.ItemName = hostatual + ".SVs.system.sv_PostInjectVis.Points[2].startPos";
                itemsList.Add(item_POSICAO_RECALQUE_mm);

                //CURSO_DOSAGEM_mm
                Item item_CURSO_DOSAGEM_mm = new Item();
                item_CURSO_DOSAGEM_mm.ItemName = hostatual + ".SVs.system.sv_rScrewCurrentPosVis";
                itemsList.Add(item_CURSO_DOSAGEM_mm);


                //CONTRAPRESSAO_MAX_bar
                //system.sv_ConstOutputBPPlastBefInj1.OutputValue
                Item item_CONTRAPRESSAO_MAX_bar = new Item();
                item_CONTRAPRESSAO_MAX_bar.ItemName = hostatual + ".SVs.system.sv_ConstOutputBPPlastBefInj1.OutputValue";
                itemsList.Add(item_CONTRAPRESSAO_MAX_bar);

                //MOLDE_FECHADO_PRESSAO_1_bar
                Item item_MOLDE_FECHADO_PRESSAO_1_bar = new Item();
                item_MOLDE_FECHADO_PRESSAO_1_bar.ItemName = hostatual + ".SVs.system.sv_MoldFwdVis.Points[1].press";
                itemsList.Add(item_MOLDE_FECHADO_PRESSAO_1_bar);

                //MOLDE_FECHADO_VELOCIDADE_1
                Item item_MOLDE_FECHADO_VELOCIDADE_1 = new Item();
                item_MOLDE_FECHADO_VELOCIDADE_1.ItemName = hostatual + ".SVs.system.sv_MoldFwdVis.Points[1].speed";
                itemsList.Add(item_MOLDE_FECHADO_VELOCIDADE_1);

                //MOLDE_FECHADO_FECHAR_MOLDE_POSICAO_1_mm
                Item item_MOLDE_FECHADO_FECHAR_MOLDE_POSICAO_1_mm = new Item();
                item_MOLDE_FECHADO_FECHAR_MOLDE_POSICAO_1_mm.ItemName = hostatual + ".SVs.system.sv_MoldFwdVis.Points[1].startPos";
                itemsList.Add(item_MOLDE_FECHADO_FECHAR_MOLDE_POSICAO_1_mm);
                
                //ABERTURA_MOLDE_POSICAO_mm
                Item item_ABERTURA_MOLDE_POSICAO_mm = new Item();
                item_ABERTURA_MOLDE_POSICAO_mm.ItemName = hostatual + ".SVs.system.sv_MoldBwdVis.Points[6].startPos";
                itemsList.Add(item_ABERTURA_MOLDE_POSICAO_mm);

                //ABERTURA_MOLDE_FLUXO
                Item item_ABERTURA_MOLDE_FLUXO = new Item();
                item_ABERTURA_MOLDE_FLUXO.ItemName = hostatual + ".SVs.system.sv_MoldBwdVis.Points[1].speed";
                itemsList.Add(item_ABERTURA_MOLDE_FLUXO);

                //ABERTURA_MOLDE_PRESSAO_bar
                Item item_ABERTURA_MOLDE_PRESSAO_bar = new Item();
                item_ABERTURA_MOLDE_PRESSAO_bar.ItemName = hostatual + ".SVs.system.sv_MoldBwdVis.Points[1].press";
                itemsList.Add(item_ABERTURA_MOLDE_PRESSAO_bar);

                
                //INJECAO_MOLDE_PRESSAO_bar
                Item item_INJECAO_MOLDE_PRESSAO_bar = new Item();
                item_INJECAO_MOLDE_PRESSAO_bar.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[1].press";
                itemsList.Add(item_INJECAO_MOLDE_PRESSAO_bar);

                //INJECAO_MOLDE_FLUXO
                Item item_INJECAO_MOLDE_FLUXO = new Item();
                item_INJECAO_MOLDE_FLUXO.ItemName = hostatual + ".SVs.system.sv_PreInjectVis.Points[1].speed";
                itemsList.Add(item_INJECAO_MOLDE_FLUXO);

                
                //RECALQUE_PRESSAO_bar
                Item item_RECALQUE_PRESSAO_bar = new Item();
                item_RECALQUE_PRESSAO_bar.ItemName = hostatual + ".SVs.system.sv_PostInjectVis.Points[1].press";
                itemsList.Add(item_RECALQUE_PRESSAO_bar);

                //RECALQUE_FLUXO
                Item item_RECALQUE_FLUXO = new Item();
                item_RECALQUE_FLUXO.ItemName = hostatual + ".SVs.system.sv_PostInjectVis.Points[1].speed";
                itemsList.Add(item_RECALQUE_FLUXO);

                //RECALQUE_TEMPO_s
                Item item_RECALQUE_TEMPO_s = new Item();
                item_RECALQUE_TEMPO_s.ItemName = hostatual + ".SVs.system.sv_InjectHoldTime";
                itemsList.Add(item_RECALQUE_TEMPO_s);

                //DOSAGEM_POSICAO_mm
                Item item_DOSAGEM_POSICAO_mm = new Item();
                item_DOSAGEM_POSICAO_mm.ItemName = hostatual + ".SVs.system.sv_rScrewCurrentPosVis";
                itemsList.Add(item_DOSAGEM_POSICAO_mm);

                //DOSAGEM_PRESSAO_bar
                Item item_DOSAGEM_PRESSAO_bar = new Item();
                item_DOSAGEM_PRESSAO_bar.ItemName = hostatual + ".SVs.system.sv_AutoPurgeParams.PlasticizingParam.ConstOutputPre.OutputValue";
                itemsList.Add(item_DOSAGEM_PRESSAO_bar);

                //DOSAGEM_FLUXO
                Item item_DOSAGEM_FLUXO = new Item();
                item_DOSAGEM_FLUXO.ItemName = hostatual + ".SVs.system.sv_AutoPurgeParams.PlasticizingParam.ConstOutputVel.OutputValue";
                itemsList.Add(item_DOSAGEM_FLUXO);

                //DOSAGEM_CONTRA_PRESSAO_bar
                Item item_DOSAGEM_CONTRA_PRESSAO_bar = new Item();
                item_DOSAGEM_CONTRA_PRESSAO_bar.ItemName = hostatual + ".SVs.system.sv_ConstOutputBPPlastBefInj1.OutputValue";
                itemsList.Add(item_DOSAGEM_CONTRA_PRESSAO_bar);

                //EXTRACAO_POSICAO_mm
                Item item_EXTRACAO_POSICAO_mm = new Item();
                item_EXTRACAO_POSICAO_mm.ItemName = hostatual + ".SVs.system.sv_rEjectCurrentPosVis";
                itemsList.Add(item_EXTRACAO_POSICAO_mm);

                //Equipamentos auxiliares 
                
                //Temperatura de Controle Aquecedor de molde 
                Item Temperatura_Controle_Aquecedor_Molde = new Item();
                Temperatura_Controle_Aquecedor_Molde.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_MoldHeaterTargetValue[1]";
                itemsList.Add(Temperatura_Controle_Aquecedor_Molde);
                
                //Temperatura água retorno Aquecedor de Molde (°C)
                Item Temperatura_Retorno_Agua_Aquecedor_Molde = new Item();
                Temperatura_Retorno_Agua_Aquecedor_Molde.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_ReturnWaterTemp[1]";
                itemsList.Add(Temperatura_Retorno_Agua_Aquecedor_Molde);

                //Temperatura da água de saída Aquecedor Molde (°C)
                Item Temperatura_Saida_Agua_Aquecedor_Molde = new Item();
                Temperatura_Saida_Agua_Aquecedor_Molde.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_OutputWaterTemp[1]";
                itemsList.Add(Temperatura_Saida_Agua_Aquecedor_Molde);

                //Temperatura Configurada Aquecedor de molde (°C)
                Item Temperatura_Configurada_Aquecedor_Molde = new Item();
                Temperatura_Configurada_Aquecedor_Molde.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_MoldHeaterSetValue[1]";
                itemsList.Add(Temperatura_Configurada_Aquecedor_Molde);
                
                //Temperatura atual do aquecedor de molde  (°C)
                Item Temperatura_Atual_Aquecedor_Molde = new Item();
                Temperatura_Atual_Aquecedor_Molde.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_MoldHeaterActValue[1]";
                itemsList.Add(Temperatura_Atual_Aquecedor_Molde);

               //Chillers (Geladeira) 
               
                // Temperatura Controle Chiller (°C) 
                Item Temperatura_Controle_Chiller = new Item();
                Temperatura_Controle_Chiller.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_CoolerTargetTemperature[1]";
                itemsList.Add(Temperatura_Controle_Chiller);

                // Temperatura Configurada Chiller (°C)
                Item Temperatura_Configurada_Chiller = new Item();
                Temperatura_Configurada_Chiller.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_CoolerSetTemperature[1]";
                itemsList.Add(Temperatura_Controle_Chiller);

                // Temperatura Atual Chillers(°C)
                Item Temperatura_Atual_Chiller = new Item();
                Temperatura_Atual_Chiller.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_CoolerActTemperature[1]";
                itemsList.Add(Temperatura_Atual_Chiller);


                //Dryer (Desumidificador) 

                // Temperatura Configurada Desumidificador (°C)
                Item Temperatura_Configurada_Desumidificador = new Item();
                Temperatura_Configurada_Desumidificador.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_DryerSetTemperature[1]";
                itemsList.Add(Temperatura_Configurada_Desumidificador);

                 // Temperatura Configurada Desumidificador (°C)
                Item Temperatura_Atual_Desumidificador = new Item();
                Temperatura_Atual_Desumidificador.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_DryerActTemperature[1]";
                itemsList.Add(Temperatura_Atual_Desumidificador);

                // Temperatura de Controle  Desumidificador (°C)
                Item Temperatura_Controle_Desumidificador = new Item();
                Temperatura_Controle_Desumidificador.ItemName = hostatual + ".SVs.AuxiliaryEquipment.sv_DryerTargetTemperature[1]";
                itemsList.Add(Temperatura_Controle_Desumidificador);
                

                groupRead.AddItems(itemsList.ToArray());
                

                groupStateWrite = new Opc.Da.SubscriptionState();
                groupStateWrite.Name =hostatual+  "myWriteGroup";
                groupStateWrite.Active = true;
                groupWrite = (Opc.Da.Subscription)serveratual.CreateSubscription(groupStateWrite);
            }
            catch (Exception exc)
            {
                 MessageBox.Show("Error ao adicionar o listener opc no host "+ hostatual);
                isListenerContinuaExecutando = false;
            }
        }

        void groupRead_DataChanged(object subscriptionHandle, object requestHandle, ItemValueResult[] values)

        {


            foreach (ItemValueResult itemValue in values)
            {
             
               if(itemValue.ItemName!=null){

                   if (itemValue.ItemName.Equals(hostInjetora+".SVs.system.sv_TempZone1"))
                   {

                       try
                       {
                           temperatura = Convert.ToDouble(itemValue.Value.ToString());
                           Console.WriteLine("O valor de temperatura no  if " + itemValue.Value.ToString() + " " + hostInjetora);

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }

                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_TempZone2"))
                   {

                       try
                       {
                           TEMPERATURA_ZONA_2 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_TempZone3"))
                   {

                       try
                       {
                           TEMPERATURA_ZONA_3 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_TempZone4"))
                   {

                       try
                       {
                           TEMPERATURA_ZONA_4 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_TempZone5"))
                   {

                       try
                       {
                           TEMPERATURA_ZONA_5 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }


                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_MoldHeaterTargetValue[1]"))
                   {

                       try
                       {
                           Temperatura_Controle_Aquecedor_Molde = Convert.ToDouble(itemValue.Value.ToString());
                           Console.WriteLine("O valor de temperatura de Controle " + itemValue.Value.ToString() + " " + hostInjetora);
                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_ReturnWaterTemp[1]"))
                   {

                       try
                       {
                           Temperatura_Agua_Retorno_Aquecedor_Molde= Convert.ToDouble(itemValue.Value.ToString());
                          // Console.WriteLine("Retorno aquecedor de molde" + itemValue.Value.ToString() + " " + hostInjetora);
                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_OutputWaterTemp[1]"))
                   {

                       try
                       {
                           Temperatura_Agua_Saida_Aquecedor_Molde = Convert.ToDouble(itemValue.Value.ToString());
                           //Console.WriteLine("Tempertura água de saída" + itemValue.Value.ToString() + " " + hostInjetora);
                           

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_MoldHeaterSetValue[1]"))
                   {

                       try
                       {
                           Temperatura_Configurada_Aquecedor_Molde = Convert.ToDouble(itemValue.Value.ToString());
                           //Console.WriteLine("Tempertura Configurada Aquecedor" + itemValue.Value.ToString() + " " + hostInjetora);


                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }


                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_MoldHeaterActValue[1]"))
                   {

                       try
                       {
                           Temperatura_Atual_Aquecedor_Molde = Convert.ToDouble(itemValue.Value.ToString());
                           //Console.WriteLine("Tempertura atual Aquecedor" + itemValue.Value.ToString() + " " + hostInjetora);


                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_CoolerTargetTemperature[1]"))
                   {

                       try
                       {
                           Temperatura_Atual_Aquecedor_Molde = Convert.ToDouble(itemValue.Value.ToString());
                           //Console.WriteLine("Tempertura atual Aquecedor" + itemValue.Value.ToString() + " " + hostInjetora);


                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }

                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_CoolerTargetTemperature[1]"))
                   {

                       try
                       {
                           Temperatura_Controle_Chiller = Convert.ToDouble(itemValue.Value.ToString());
                          

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_CoolerSetTemperature[1]"))
                   {

                       try
                       {
                           Temperatura_Configurada_Chiller = Convert.ToDouble(itemValue.Value.ToString());


                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_CoolerActTemperature[1]"))
                   {

                       try
                       {
                           Temperatura_Atual_Chillers = Convert.ToDouble(itemValue.Value.ToString());


                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }

                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_DryerSetTemperature[1]"))
                   {

                       try
                       {
                           Temperatura_Configurada_Desumidificador = Convert.ToDouble(itemValue.Value.ToString());


                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_DryerActTemperature[1]"))
                   {

                       try
                       {
                           Temperatura_Atual_Desumidificador = Convert.ToDouble(itemValue.Value.ToString());


                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.AuxiliaryEquipment.sv_DryerTargetTemperature[1]"))
                   {

                       try
                       {
                           Temperatura_Controle_Desumidificador = Convert.ToDouble(itemValue.Value.ToString());


                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }  
                   
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_InjectTime"))
            {

                  try
                        {
                            TEMPO_INJECAO_REAL_s = Convert.ToDouble(itemValue.Value.ToString());
                            TEMPO_INJECAO_REAL_s = Math.Round(TEMPO_INJECAO_REAL_s / CONVERTER, 2);
                            if (TEMPO_INJECAO_REAL_s >= TEMPO_INJECAO_REAL_MAX_s)
                            {
                                TEMPO_INJECAO_REAL_MAX_s = TEMPO_INJECAO_REAL_s;

                            }
                            Console.Out.WriteLine("LeituraOPC");
                            Console.Out.WriteLine(TEMPO_INJECAO_REAL_MAX_s.ToString());
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(1000);
                        }

            
                   } else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[1].press")) {
            
                        try
                        {
                            PRESSAO_INJECAO = Convert.ToDouble(itemValue.Value.ToString());
                            INJECAO_MOLDE_PRESSAO_bar = Convert.ToDouble(itemValue.Value.ToString());
                            PRESSAO_INJECAO_MAX_bar = Convert.ToDouble(itemValue.Value.ToString());
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(1000);
                        }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[2].press"))
                   {

                       try
                       {
                           PRESSAO_INJECAO_2= Convert.ToDouble(itemValue.Value.ToString());
                           
                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[3].press"))
                   {

                       try
                       {
                           PRESSAO_INJECAO_3 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[4].press"))
                   {

                       try
                       {
                           PRESSAO_INJECAO_4 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[5].press"))
                   {

                       try
                       {
                           PRESSAO_INJECAO_5 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[6].press"))
                   {

                       try
                       {
                           PRESSAO_INJECAO_6 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }

                   }

                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[1].speed"))
                   {

                   try
                   {
                       VELOCIDADE_INJECAO_1 = Convert.ToDouble(itemValue.Value.ToString());
                       INJECAO_MOLDE_FLUXO = Convert.ToDouble(itemValue.Value.ToString());
                   }
                   catch (Exception ex)
                   {
                       log.Info("Erro na chamada do webservice: " + ex);
                       Thread.Sleep(1000);
                   }


                   }
                  
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[2].speed"))
                   {

                       try
                       {
                           VELOCIDADE_INJECAO_2 = Convert.ToDouble(itemValue.Value.ToString());
                          
                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[3].speed"))
                   {

                       try
                       {
                           VELOCIDADE_INJECAO_3 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[4].speed"))
                   {

                       try
                       {
                           VELOCIDADE_INJECAO_4 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[5].speed"))
                   {

                       try
                       {
                           VELOCIDADE_INJECAO_5 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[6].speed"))
                   {

                       try
                       {
                           VELOCIDADE_INJECAO_6= Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }

                       // Posição de injeção
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[1].startPos"))
                   {

                       try
                       {
                           POSICAO_INJECAO_1 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }   
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[2].startPos"))
                   {

                       try
                       {
                           POSICAO_INJECAO_2 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }
                
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[3].startPos"))
                   {

                       try
                       {
                           POSICAO_INJECAO_3 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }   
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[4].startPos"))
                   {

                       try
                       {
                           POSICAO_INJECAO_4 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }
                
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PreInjectVis.Points[5].startPos"))
                   {

                       try
                       {
                           POSICAO_INJECAO_5 = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_PostInjectVis.Points[1].press"))
                   {

                       try
                        {
                            PRESSAO_RECALQUE_bar = Convert.ToDouble(itemValue.Value.ToString());
                            RECALQUE_PRESSAO_bar = Convert.ToDouble(itemValue.Value.ToString());
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(1000);
                        }


                   }else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_InjectHoldTime"))
                   {

                     try
                        {
                            TEMPO_RECALQUE_s = Convert.ToDouble(itemValue.Value.ToString());
                            TEMPO_RECALQUE_s = Math.Round(TEMPO_RECALQUE_s / CONVERTER, 2);
                            RECALQUE_TEMPO_s = TEMPO_RECALQUE_s;
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(1000);
                        }


                   }else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_EjectorMoveTime"))
                   {

                     try
                        {
                            TEMPO_EXTRACAO_s = Convert.ToDouble(itemValue.Value.ToString());
                            TEMPO_EXTRACAO_s = Math.Round(TEMPO_EXTRACAO_s / CONVERTER, 2);
                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(1000);
                        }


                   }else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_ChargeTime"))   
                   {
                  try
                        {
                            TEMPO_DOSAGEM_s = Convert.ToDouble(itemValue.Value.ToString());
                            TEMPO_DOSAGEM_s = Math.Round(TEMPO_DOSAGEM_s / CONVERTER, 2);

                        }
                        catch (Exception ex)
                        {
                            log.Info("Erro na chamada do webservice: " + ex);
                            Thread.Sleep(1000);
                        }


                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_CoolingTime"))
                   {
                       try
                       {
                           TEMPO_RESFRIAMENTO_s = Convert.ToDouble(itemValue.Value.ToString());
                           TEMPO_RESFRIAMENTO_s = Math.Round(TEMPO_RESFRIAMENTO_s / CONVERTER, 2);
                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }

                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_MoldProtectionTime"))
                   {
                       try
                       {
                           TEMPO_PROTECAO_MOLDE_s = Convert.ToDouble(itemValue.Value.ToString());
                           TEMPO_PROTECAO_MOLDE_s = Math.Round(TEMPO_PROTECAO_MOLDE_s / CONVERTER, 2);
                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }


                   }else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_ConstOutputBPPlastBefInj1.OutputValue"))
                   {
                      try
                      {
                          CONTRAPRESSAO_MAX_bar = Convert.ToDouble(itemValue.Value.ToString());
                          DOSAGEM_CONTRA_PRESSAO_bar = Convert.ToDouble(itemValue.Value.ToString());
                      }
                      catch (Exception ex)
                      {
                          log.Info("Erro na chamada do webservice: " + ex);
                          Thread.Sleep(1000);
                      }
                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_AutoPurgeParams.PlasticizingParam.ConstOutputPre.OutputValue"))
                   {
                       try
                       {
                           DOSAGEM_PRESSAO_bar = Convert.ToDouble(itemValue.Value.ToString());
                           
                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }
                   }
                   else if (itemValue.ItemName.Equals(hostInjetora + ".SVs.system.sv_rScrewCurrentPosVis"))
                   {
                       try
                       {
                           DOSAGEM_POSICAO_mm = Convert.ToDouble(itemValue.Value.ToString());

                       }
                       catch (Exception ex)
                       {
                           log.Info("Erro na chamada do webservice: " + ex);
                           Thread.Sleep(1000);
                       }
                   }
                 
                 



                //string teste = itemValue.ItemName;

                //switch (itemValue.ItemName.rea)
                //{

                    
                //    case "SVs.system.sv_PreInjectVis.Points[2].startPos":

                //        try
                //        {
                //            INJECAO_MOLDE_POSICAO_mm = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }


                //        break;
                //    case "teste.SVs.system.sv_TempZone1":
                //        Console.Out.WriteLine(itemValue.Value.ToString());
                       
                //        //itemValue.ToString();
                //        try
                //        {
                //           // Console.WriteLine("O valor de temperatura " + itemValue.Value.ToString() + " " + hostInjetora);
                //            TEMPERATURA_ZONA_2 = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }


                //        break;

                //    case ".SVs.system.sv_TempZone3":
                //        //Console.Out.WriteLine(itemValue.Value.ToString());
                //        //itemValue.ToString();
                //        try
                //        {
                //            TEMPERATURA_ZONA_3 = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }


                //        break;



                //    //case "SVs.system.sv_PreInjectVis.Points[1].press":

                //    //    try
                //    //    {
                //    //        PRESSAO_INJECAO = Convert.ToDouble(itemValue.Value.ToString());
                //    //        INJECAO_MOLDE_PRESSAO_bar = Convert.ToDouble(itemValue.Value.ToString());
                //    //        PRESSAO_INJECAO_MAX_bar = Convert.ToDouble(itemValue.Value.ToString());
                //    //    }
                //    //    catch (Exception ex)
                //    //    {
                //    //        log.Info("Erro na chamada do webservice: " + ex);
                //    //        Thread.Sleep(1000);
                //    //    }


                //    //    break;

                //    case "SVs.system.sv_MoldFwdVis.Points[1].press":
                //        //itemValue.ToString();


                //        try
                //        {
                //            PRESSAO_MOLDE_bar = Convert.ToDouble(itemValue.Value.ToString());
                //            PRESSAO_FORCA_FECHAMENTO_bar = Convert.ToDouble(itemValue.Value.ToString());
                //            MOLDE_FECHADO_PRESSAO_1_bar = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    case "SVs.system.sv_PostInjectVis.Points[1].press":
                //        //itemValue.ToString();
                //        try
                //        {
                //            PRESSAO_RECALQUE_bar = Convert.ToDouble(itemValue.Value.ToString());
                //            RECALQUE_PRESSAO_bar = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    //case "SVs.system.sv_MoldFwdVis.Points[1].press":
                //    //itemValue.ToString();
                //    //PRESSAO_FORCA_FECHAMENTO_bar = Convert.ToDouble(itemValue.Value.ToString());
                //    //break;

                //    case "SVs.system.sv_MoldFwdVis.Points[4].press":
                //        //itemValue.ToString();
                //        try
                //        {
                //            PRESSAO_PROTECAO_MOLDE_bar = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    case "SVs.system.sv_InjectTime":

                //        //Console.Out.WriteLine(itemValue.Value.ToString());

                //        try
                //        {
                //            TEMPO_INJECAO_REAL_s = Convert.ToDouble(itemValue.Value.ToString());
                //            TEMPO_INJECAO_REAL_s = Math.Round(TEMPO_INJECAO_REAL_s / CONVERTER, 2);
                //            if (TEMPO_INJECAO_REAL_s >= TEMPO_INJECAO_REAL_MAX_s)
                //            {
                //                TEMPO_INJECAO_REAL_MAX_s = TEMPO_INJECAO_REAL_s;

                //            }
                //            Console.Out.WriteLine("LeituraOPC");
                //            Console.Out.WriteLine(TEMPO_INJECAO_REAL_MAX_s.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    case "SVs.system.sv_InjectHoldTime":
                //        try
                //        {
                //            TEMPO_RECALQUE_s = Convert.ToDouble(itemValue.Value.ToString());
                //            TEMPO_RECALQUE_s = Math.Round(TEMPO_RECALQUE_s / CONVERTER, 2);
                //            RECALQUE_TEMPO_s = TEMPO_RECALQUE_s;
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    case "SVs.system.sv_MoldOpenTime":

                //        try
                //        {
                //            TEMPO_ABERTURA_PLACA_MOVEL_s = Convert.ToDouble(itemValue.Value.ToString());
                //            TEMPO_ABERTURA_PLACA_MOVEL_s = Math.Round(TEMPO_ABERTURA_PLACA_MOVEL_s / CONVERTER, 2);
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }


                //        //itemValue.ToString();,
                //        break;

                //    case "SVs.system.sv_MoldCloseTime":

                //        try
                //        {
                //            TEMPO_FECHAMENTO_PLACA_MOVEL_s = Convert.ToDouble(itemValue.Value.ToString());
                //            TEMPO_FECHAMENTO_PLACA_MOVEL_s = Math.Round(TEMPO_FECHAMENTO_PLACA_MOVEL_s / CONVERTER, 2);
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }


                //        //itemValue.ToString();,
                //        break;

                //    case "SVs.system.sv_EjectorMoveTime":

                //        try
                //        {
                //            TEMPO_EXTRACAO_s = Convert.ToDouble(itemValue.Value.ToString());
                //            TEMPO_EXTRACAO_s = Math.Round(TEMPO_EXTRACAO_s / CONVERTER, 2);
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }



                //        //itemValue.ToString();,
                //        break;

                //    case "SVs.system.sv_ChargeTime":
                //        try
                //        {
                //            TEMPO_DOSAGEM_s = Convert.ToDouble(itemValue.Value.ToString());
                //            TEMPO_DOSAGEM_s = Math.Round(TEMPO_DOSAGEM_s / CONVERTER, 2);

                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }


                //        break;


                //    case "SVs.system.sv_CoolingTime":
                //        try
                //        {
                //            TEMPO_RESFRIAMENTO_s = Convert.ToDouble(itemValue.Value.ToString());
                //            TEMPO_RESFRIAMENTO_s = Math.Round(TEMPO_RESFRIAMENTO_s / CONVERTER, 2);
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    case "SVs.system.sv_MoldProtectionTime":
                //        try
                //        {
                //            TEMPO_PROTECAO_MOLDE_s = Convert.ToDouble(itemValue.Value.ToString());
                //            TEMPO_PROTECAO_MOLDE_s = Math.Round(TEMPO_PROTECAO_MOLDE_s / CONVERTER, 2);
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }



                //        break;

                //    case "SVs.system.sv_PreInjectVis.Points[1].speed":
                //        try
                //        {
                //            VELOCIDADE_INJECAO_1 = Convert.ToDouble(itemValue.Value.ToString());
                //            INJECAO_MOLDE_FLUXO = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        //itemValue.ToString();
                //        break;

                //    case "SVs.system.sv_PreInjectVis.Points[2].speed":

                //        try
                //        {
                //            VELOCIDADE_INJECAO_2 = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    case "SVs.system.sv_PreInjectVis.Points[3].speed":
                //        try
                //        {
                //            VELOCIDADE_INJECAO_3 = Convert.ToDouble(itemValue.Value.ToString());
                //            //itemValue.ToString();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }


                //        break;

                //    case "SVs.system.sv_PreInjectVis.Points[4].speed":
                //        try
                //        {
                //            VELOCIDADE_INJECAO_4 = Convert.ToDouble(itemValue.Value.ToString());

                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    case "SVs.system.sv_PreInjectVis.Points[5].speed":
                //        try
                //        {
                //            VELOCIDADE_INJECAO_5 = Convert.ToDouble(itemValue.Value.ToString());

                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        //itemValue.ToString();
                //        break;

                //    case "SVs.system.sv_PreInjectVis.Points[6].speed":

                //        try
                //        {
                //            VELOCIDADE_INJECAO_6 = Convert.ToDouble(itemValue.Value.ToString());

                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    case "SVs.system.sv_PostInjectVis.Points[2].startPos":
                //        try
                //        {
                //            POSICAO_RECALQUE_mm = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        //itemValue.ToString();
                //        break;


                //    case "SVs.system.sv_rScrewCurrentPosVis":

                //        try
                //        {
                //            DOSAGEM_POSICAO_mm = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        //itemValue.ToString();
                //        break;

                //    

                //        break;

                //    //case "SVs.system.sv_MoldFwdVis.Points[1].press":

                //    // MOLDE_FECHADO_PRESSAO_1_bar = Convert.ToDouble(itemValue.Value.ToString());
                //    //itemValue.ToString();
                //    // break;
                //    case "SVs.system.sv_rEjectCurrentPosVis":


                //        try
                //        {
                //            EXTRACAO_POSICAO_mm = Convert.ToDouble(itemValue.Value.ToString());
                //            //itemValue.ToString();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        break;

                //    case "SVs.system.sv_MoldFwdVis.Points[1].startPos":

                //        try
                //        {
                //            MOLDE_FECHADO_FECHAR_MOLDE_POSICAO_1_mm = Convert.ToDouble(itemValue.Value.ToString());
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }


                //        //itemValue.ToString();
                //        break;

                //    case "SVs.system.sv_MoldFwdVis.Points[1].speed":

                //        try
                //        {
                //            MOLDE_FECHADO_VELOCIDADE_1 = Convert.ToDouble(itemValue.Value.ToString());
                //            //itemValue.ToString();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }



                //        //itemValue.ToString();
                //        break;

                //    case "SVs.system.sv_MoldBwdVis.Points[1].startPos":

                //        try
                //        {
                //            ABERTURA_MOLDE_POSICAO_mm = Convert.ToDouble(itemValue.Value.ToString());
                //            //itemValue.ToString();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }
                //        //itemValue.ToString();
                //        break;

                //    case "SVs.system.sv_MoldBwdVis.Points[1].press":

                //        try
                //        {
                //            ABERTURA_MOLDE_PRESSAO_bar = Convert.ToDouble(itemValue.Value.ToString());
                //            //itemValue.ToString();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }


                //        //itemValue.ToString();
                //        break;

                //    case "SVs.system.sv_MoldBwdVis.Points[1].speed":
                //        try
                //        {
                //            ABERTURA_MOLDE_FLUXO = Convert.ToDouble(itemValue.Value.ToString());
                //            //itemValue.ToString();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        //itemValue.ToString();
                //        break;

                //    case "SVs.system.sv_PreInjectVis.Points[1].startPos":
                //        try
                //        {
                //            INJECAO_MOLDE_POSICAO_mm = Convert.ToDouble(itemValue.Value.ToString());
                //            //itemValue.ToString();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        //itemValue.ToString();
                //        break;

                //    case "SVs.system.sv_PostInjectVis.Points[1].speed":
                //        try
                //        {
                //            RECALQUE_FLUXO = Convert.ToDouble(itemValue.Value.ToString());
                //            //itemValue.ToString();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        //itemValue.ToString();
                //        break;
                //    case "SVs.system.sv_AutoPurgeParams.PlasticizingParam.ConstOutputPre.OutputValue":
                //        try
                //        {
                //            //PRESSAO_INJECAO = 105.0;
                //            DOSAGEM_PRESSAO_bar = Convert.ToDouble(itemValue.Value.ToString());

                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        //itemValue.ToString();
                //        break;

                //    case "SVs.system.sv_AutoPurgeParams.PlasticizingParam.ConstOutputVel.OutputValue":
                //        try
                //        {
                //            DOSAGEM_FLUXO = Convert.ToDouble(itemValue.Value.ToString());
                //            //itemValue.ToString();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Info("Erro na chamada do webservice: " + ex);
                //            Thread.Sleep(1000);
                //        }

                //        //itemValue.ToString();
                //        break;

                //    ////Verificar Alertas Pendentes
                //    //case "SVs.system.sv_PendingAlarms":

                //    //    try
                //    //    {
                //    //        int status = (int)itemValue.Value;
                //    //        if (status > 0)
                //    //        {
                //    //            //lerAlertasOPC();
                //    //        }
                //    //    }
                //    //    catch (Exception ex)
                //    //    {
                //    //        Thread.Sleep(1000);
                //    //    }


                //    //    break;

                //    //case "Funcs.Alarm.ReadAlarms().Out.Alarms":
                //    //    try
                //    //    {
                //    //        object[] alarmes;
                //    //        // = new string[45][];
                //    //        alarmes = (object[])itemValue.Value;


                //    //        object[] amostra;
                //    //        amostra = (object[])alarmes[0];


                //    //        for (int i = 0; i < alarmes.Length; i++)
                //    //        {

                //    //            try
                //    //            {
                //    //                amostra = (object[])alarmes[i];
                //    //                Console.Out.WriteLine("Os id dos alarmes : " + amostra[4].ToString());
                //    //                //validarAlertas(amostra[4].ToString());
                //    //            }
                //    //            catch (Exception e)
                //    //            {

                //    //            }
                //    //        }
                //    //    }
                //    //    catch (Exception e)
                //    //    {

                //    //    }


                        //break;

               
                }
            }
        }

        public void finalizaThreads()
        {
            log.Info("Chamando metodo finalizaThreads()");

            isListenerContinuaExecutando = false;

        }


        // Escrever no opc
        private void WriteData(string itemName, int value)
        {
            groupWrite.RemoveItems(groupWrite.Items);
            List<Item> writeList = new List<Item>();
            List<ItemValue> valueList = new List<ItemValue>();

            Item itemToWrite = new Item();
            itemToWrite.ItemName = itemName;
            ItemValue itemValue = new ItemValue(itemToWrite);
            itemValue.Value = value;

            writeList.Add(itemToWrite);
            valueList.Add(itemValue);
            //IMPORTANT:
            //#1: assign the item to the group so the items gets a ServerHandle
            groupWrite.AddItems(writeList.ToArray());
            // #2: assign the server handle to the ItemValue
            for (int i = 0; i < valueList.Count; i++)
                valueList[i].ServerHandle = groupWrite.Items[i].ServerHandle;
            // #3: write
            groupWrite.Write(valueList.ToArray());
        }

        public void reconectarOpcServer(Server serverAtualizado,string hostatual)
        {
          try
            {
                serverAtual.Disconnect();
                ConnectToOpcServer(hostatual, serverAtual);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha no host"+hostInjetora);
            }
            
        }
    }
}
