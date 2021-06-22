using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Opc.Da;
using System.Drawing;
using System.Linq;
using System.Text;
using ColetaInjetoraToshiba.webservice;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using ColetaInjetoraToshiba.coleta;
using ColetaInjetoraToshiba.util;
using NLog;
using System.Web.Services;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ColetaInjetoraToshiba.tela
{
    public partial class TelaPrincipal : Form
    {
        private TcpClient tc = null;
        private NetworkStream ns;
        ArrayList posto_cadastrado = null;
        msws.icDTO icdto = null;
        msws.icUpDTO icupdto = null;
        private Logger log = LogManager.GetCurrentClassLogger();
        private Server server;
        private OpcCom.Factory fact;
        TcpListener serverTcp = null;
        byte[] vetorRequisicao = new byte[18];

        //Parâmetros da folha: 
        private String titulo = "";
        private String date = "";
        private String codigo = "";
        private String VI1 = "0.0";
        private String VI2 = "0.0";
        private String VI3 = "0.0";
        private String VI4 = "0.0";
        private String VI5 = "0.0";
        private String VI6 = "0.0";
        private String VI7 = "0.0";
        private String VI8 = "0.0";
        private String VI9 = "0.0";
        private String VI10 = "0.0";
        private String VH1 = "0.0";
        private String VH2 = "0.0";
        private String PI1 = "0.0";
        private String PI2 = "0.0";
        private String PH1 = "0.0";
        private String PH2 = "0.0";
        private String PH3 = "0.0";
        private String PH4 = "0.0";
        private String SRN = "0.0";
        private String BP = "0.0";
        private String LS4 = "0.0";
        private String LS4A = "0.0";
        private String LS4B = "0.0";
        private String LS4C = "0.0";
        private String LS4D = "0.0";
        private String LS4E = "0.0";
        private String LS5 = "0.0";


        private String PRODUCTION_SHOT = "0.0";        
        private String DWELL_PRESSURE = "0.0";
        private String OK_PRODUCTION_SHOT = "0.0";
        private String PRING_SHOT = "0.0";
        private String FILLING_TIME = "0.0";
        private String CHARGIN_TIME = "0.0";
        private String TAKE_OUT_TIME = "0.0";
        private String CYCLE_TIME = "0.0";
        private String MINIMUN_CUSHION_POSITION = "0.0";
        private String CUSHION_POSITION_ = "0.0";
        private String DWELL_CHANGE_POSITION = "0.0";
        private String INJECTION_START_POSITION = "0.0";
        private String MAXIMUM_INJECTION_PRESSURE = "0.0";
        private String DEWELL_PRESSURE = "0.0";
        private String SCREW_ROTATION_SPEED = "0.0";

        private String TEMPERATURE_HEN = "0.0";
        private String TEMPERATURE_HN = "0.0";
        private String TEMPERATURE_H1 = "0.0";
        private String TEMPERATURE_H2 = "0.0";
        private String TEMPERATURE_H3 = "0.0";
        private String TEMPERATURE_H4 = "0.0";
        private String TEMPERATURE_H5 = "0.0";
        private String TEMPERATURE_OIL = "0.0";
        private String TEMPERATURE_HOP = "0.0";



        private String ipServidorMap;



        public TelaPrincipal()
        {
            InitializeComponent();


            
        }

        public void iniciaColeta(String maqIp, String serverIp, String prog) {           
        
            string mac = "5C:F3:FC:FD:E1:F5";
           // listMaquinas.Text = "Thread Iniciada";
            string ipToshiba = maqIp;

            int porta = 120;

            IPAddress local = IPAddress.Parse(serverIp);
           // listMaquinas.Text = "IP: " + ip;
            ////FLAG - READ
            vetorRequisicao[0] = 0x30;
            vetorRequisicao[1] = 0x32;

            //CMD - MOLDING CONDITION REQUEST
            vetorRequisicao[2] = 0x30;
            vetorRequisicao[3] = 0x30;
            vetorRequisicao[4] = 0x30;
            vetorRequisicao[5] = 0x31;

            ////LEN - 4 BYTES
            vetorRequisicao[6] = 0x30;
            vetorRequisicao[7] = 0x30;

            vetorRequisicao[8] = 0x30;
            vetorRequisicao[9] = 0x30;

            vetorRequisicao[10] = 0x30;
            vetorRequisicao[11] = 0x30;

            vetorRequisicao[12] = 0x30;
            vetorRequisicao[13] = 0x34;

            int data = Int32.Parse(prog);
            byte number = Convert.ToByte(data);

            //listMaquinas.Text = number.ToString();

            ////DATA
            vetorRequisicao[14] = 0x0;
            vetorRequisicao[15] = 0x0;

            vetorRequisicao[16] = 0x0;
            vetorRequisicao[17] = number;

            listMaquinas.Items.Add(number);
            //vetorRequisicao[17] = 0xa4;

            serverTcp = new TcpListener(local, porta);
            serverTcp.Start();

            try
            {
                bool isConectado = true;
                while (isConectado)
                {
                    try
                    {
                        //tc.Connect(ipToshiba, 139);
                        //isConectado = false;
                       // listMaquinas.Text = "Esperando por conexão";
                        Console.WriteLine("Esperando por conexão");
                        listMaquinas.Items.Add("Esperando por conexão");
                        TcpClient client = serverTcp.AcceptTcpClient();
                        Console.WriteLine("Conectado");
                        listMaquinas.Items.Add("Conectado");
                        // listMaquinas.Text = "Conectado";
                        Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                        t.Start(client);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.ToString());
                        serverTcp.Stop();
                    }

                }

                Console.WriteLine("Abri a conexão. IP: " + ipToshiba);
               // listMaquinas.Text = "Abri a conexão. IP: " + ipToshiba;
                Console.WriteLine("Request:");
                Console.WriteLine(vetorRequisicao[0]);
                Console.WriteLine(vetorRequisicao[1]);
                Console.WriteLine(vetorRequisicao[2]);
                Console.WriteLine(vetorRequisicao[3]);
                Console.WriteLine(vetorRequisicao[4]);
                Console.WriteLine(vetorRequisicao[5]);
                Console.WriteLine(vetorRequisicao[6]);
                Console.WriteLine(vetorRequisicao[7]);
                Console.WriteLine(vetorRequisicao[8]);
                Console.WriteLine(vetorRequisicao[9]);
                Console.WriteLine(vetorRequisicao[10]);
                Console.WriteLine(vetorRequisicao[11]);
                Console.WriteLine(vetorRequisicao[12]);
                Console.WriteLine(vetorRequisicao[13]);
                Console.WriteLine(vetorRequisicao[14]);
                Console.WriteLine(vetorRequisicao[15]);
                Console.WriteLine(vetorRequisicao[16]);
                Console.WriteLine(vetorRequisicao[17]);
                Console.WriteLine("Data read:");
                ns = tc.GetStream();

                if (ns.CanWrite)
                {
                    ns.Write(vetorRequisicao, 0, 30);
                }
                int count = 0;
                while (count < 50)
                {
                    count++;
                    if (ns.DataAvailable)
                    {
                        Console.WriteLine(ns.ReadByte());
                    }
                }
                ns.Close();
                tc.Close();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //listMaquinas.Text = "Iniciando Thread";
            Thread t = new Thread(NovaThread);
            t.Start();

            //iniciaColeta("123.123.123.190");            

        }

        public void NovaThread()
        {
            ipServidorMap = txtIpServidor.Text;
            iniciaColeta(textBoxMac.Text, txtIpServidor.Text, txtCodigo.Text);
        }

        public void HandleClient(Object obj) {


            TcpClient client = (TcpClient)obj;

            String ipMaquina = textBoxMac.Text;
            String programa = txtCodigo.Text;

            try {
                var stream = client.GetStream();
            } catch(Exception ex){
                Console.WriteLine(ex);
            }
            
            string imei = String.Empty;

            string data = null;
            Byte[] bytes = new Byte[1514];
            
            int i;
            bool isEscrita = false;
            var stream1 = client.GetStream();
            try { 
                //while((i = stream1.Read(bytes,0,bytes.Length)) < 50000){
                while(true){

                    String ipServer = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();               

                    i = stream1.Read(bytes, 0, bytes.Length);
                    Console.WriteLine(i);

                    if (!isEscrita) {
                        String hex = BitConverter.ToString(bytes);
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("{1}: Received {0}", data, Thread.CurrentThread.ManagedThreadId);
                    }
                    

                    if (!isEscrita){
                        stream1.Write(vetorRequisicao, 0, vetorRequisicao.Length);
                        Console.WriteLine("{1}:Enviado:{0}", vetorRequisicao.ToString(), Thread.CurrentThread.ManagedThreadId);
                        isEscrita = true;
                    }

                    if (isEscrita)
                    {
                        String hex = BitConverter.ToString(bytes);
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        //Console.WriteLine("{1}: Received {0}", data, Thread.CurrentThread.ManagedThreadId);
                    }

                    if(ipMaquina == ipServer)
                    {
                        if (bytes[0] == 0x30 && bytes[1] == 0x31 && bytes[2] == 0x30 && bytes[3] == 0x30 && bytes[4] == 0x30 && bytes[5] == 0x31)
                        {
                            Console.WriteLine("Entrou no pacote Data" + ipServer);
                            listMaquinas.Items.Add("Entrou no pacote Data" + ipServer);
                            Console.WriteLine(data[50]);
                            //Console.WriteLine(data);
                            string diahex = bytes[50].ToString("X");
                            string meshex = bytes[51].ToString("X");
                            string ano1hex = bytes[52].ToString("X");
                            string ano2hex = bytes[53].ToString("X");

                            //int dia = int.Parse(diahex, System.Globalization.NumberStyles.HexNumber);
                            titulo = data.Substring(18, 19);
                            date = diahex + "-" + meshex + "-" + ano1hex + ano2hex;

                            //incia em 230 (no caso fica 233 - 230)
                            String viHex = bytes[231].ToString("X") + bytes[230].ToString("X");
                            int vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            String viAux = vi.ToString();
                            if (vi > 0)
                                VI1 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VI2 (234 e 235)
                            viHex = bytes[235].ToString("X") + bytes[234].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VI2 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VI3 (238 e 239)
                            viHex = bytes[239].ToString("X") + bytes[238].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VI3 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VI4 (242 e 243)
                            viHex = bytes[243].ToString("X") + bytes[242].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VI4 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VI5 (246 e 247)
                            viHex = bytes[247].ToString("X") + bytes[246].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VI5 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VI6 (250 e 251)
                            viHex = bytes[251].ToString("X") + bytes[250].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VI6 = viAux.Substring(0, viAux.Length - 1) + ".0";



                            //VI7 (254 e 255)
                            viHex = bytes[255].ToString("X") + bytes[254].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VI7 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VI8 (258 e 259)
                            viHex = bytes[259].ToString("X") + bytes[258].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VI8 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VI9 (262 e 263)
                            viHex = bytes[263].ToString("X") + bytes[262].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VI9 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VI10 (266 e 267)
                            viHex = bytes[267].ToString("X") + bytes[266].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VI10 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VH1 (270 e 271)
                            viHex = bytes[271].ToString("X") + bytes[270].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VH1 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //VH2 (274 e 275)
                            viHex = bytes[275].ToString("X") + bytes[274].ToString("X");
                            vi = int.Parse(viHex, System.Globalization.NumberStyles.HexNumber);
                            viAux = vi.ToString();
                            if (vi > 0)
                                VH2 = viAux.Substring(0, viAux.Length - 1) + ".0";

                            //PI1 (278, 279, 280)

                            String piHex = bytes[280].ToString("X") + bytes[279].ToString("X") + bytes[278].ToString("X");
                            int pi = int.Parse(piHex, System.Globalization.NumberStyles.HexNumber);
                            String piAux = pi.ToString();
                            if (pi > 0)
                            {
                                if (pi > 10000)
                                {
                                    PI1 = piAux.Substring(0, piAux.Length - 2) + ".00";
                                }
                                else if (pi > 1000)
                                {
                                    PI1 = piAux.Substring(0, piAux.Length - 2) + ".00";
                                }
                                else
                                {
                                    PI1 = piAux.Substring(0, piAux.Length - 1) + ".0";
                                }
                            }


                            //LS4 (318 e 319)
                            String lsHex = bytes[321].ToString("X") + bytes[320].ToString("X") + bytes[319].ToString("X") + bytes[318].ToString("X");
                            int ls = int.Parse(lsHex, System.Globalization.NumberStyles.HexNumber);
                            String lsAux = ls.ToString();
                            if (ls > 0)
                                LS4 = lsAux.Substring(0, lsAux.Length - 3) + ".0";

                            //LS4A (322 e 323)
                            lsHex = bytes[325].ToString("X") + bytes[324].ToString("X") + bytes[323].ToString("X") + bytes[322].ToString("X");
                            ls = int.Parse(lsHex, System.Globalization.NumberStyles.HexNumber);
                            lsAux = ls.ToString();
                            if (ls > 0)
                            {
                                if (ls > 10000)
                                {
                                    LS4A = lsAux.Substring(0, lsAux.Length - 3) + ".00";
                                }
                                else if (ls > 1000)
                                {
                                    LS4A = lsAux.Substring(0, lsAux.Length - 2) + ".00";
                                }
                                else
                                {
                                    LS4A = lsAux.Substring(0, lsAux.Length - 1) + ".0";
                                }
                            }



                            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ipServidorMap + ":8081/parametros/insert");
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";

                            listMaquinas.Items.Add("enviando pacote de dados para o servidor");
                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                string json = "{\"mac\":\"" + ipServer + "\"" + "," +
                      "\"VI1\":\"" + VI1 + "\"" + "," +
                      "\"VI2\":\"" + VI2 + "\"" + "," +
                      "\"VI3\":\"" + VI3 + "\"" + "," +
                      "\"VI4\":\"" + VI4 + "\"" + "," +
                      "\"VI5\":\"" + VI5 + "\"" + "," +
                      "\"VI6\":\"" + VI6 + "\"" + "," +
                      "\"VI7\":\"" + VI7 + "\"" + "," +
                      "\"VI8\":\"" + VI8 + "\"" + "," +
                      "\"VI9\":\"" + VI9 + "\"" + "," +
                      "\"VI10\":\"" + VI10 + "\"" + "," +
                      "\"VH1\":\"" + VH1 + "\"" + "," +
                      "\"VH2\":\"" + VH2 + "\"" + "," +
                      "\"PI1\":\"" + PI1 + "\"" + "," +
                      "\"LS4\":\"" + LS4 + "\"" + "," +
                      "\"LS4A\":\"" + LS4A + "\""
                      + "}";

                                streamWriter.Write(json);
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();






                            //Print
                            Console.WriteLine("Título:" + titulo + "\n" + "Data:" + date + "\n" + "VI_1:" + VI1);
                            Console.WriteLine("VI_2:" + VI2);
                            Console.WriteLine("LS4:" + LS4);
                            Console.WriteLine("LS4A:" + LS4A);
                            Console.WriteLine("Fim do teste!");

                        }



                        if (bytes[0] == 0x30 && bytes[1] == 0x31 && bytes[2] == 0x30 && bytes[3] == 0x30 && bytes[4] == 0x31 && bytes[5] == 0x31)
                        {
                            Console.WriteLine("Entrou no pacote de qualidade " + ipServer);

                            listMaquinas.Items.Add("Entrou no pacote de qualidade" + ipServer);

                            //incia em 14 (no caso fica 17 - 14)
                            String prodShot = bytes[17].ToString("X") + bytes[16].ToString("X") + bytes[15].ToString("X") + bytes[14].ToString("X");
                            int int_prodShot = int.Parse(prodShot, System.Globalization.NumberStyles.HexNumber);
                            String prodShotAux = int_prodShot.ToString();
                            PRODUCTION_SHOT = prodShotAux;

                            //incia em 18 (no caso fica 21 - 18)
                            String ok_prodShot = bytes[21].ToString("X") + bytes[20].ToString("X") + bytes[19].ToString("X") + bytes[18].ToString("X");
                            int int_ok_prodShot = int.Parse(ok_prodShot, System.Globalization.NumberStyles.HexNumber);
                            String okprodShotAux = int_ok_prodShot.ToString();
                            OK_PRODUCTION_SHOT = okprodShotAux;

                            //incia em 26 (no caso fica 29 - 26)
                            String printShot = bytes[29].ToString("X") + bytes[28].ToString("X") + bytes[27].ToString("X") + bytes[26].ToString("X");
                            int int_printShot = int.Parse(printShot, System.Globalization.NumberStyles.HexNumber);
                            String printShotAux = int_printShot.ToString();
                            PRING_SHOT = printShotAux;

                            //incia em 30 (no caso fica 33 - 30)
                            String fillingTime = bytes[33].ToString("X") + bytes[32].ToString("X") + bytes[31].ToString("X") + bytes[30].ToString("X");
                            int int_fillingTime = int.Parse(fillingTime, System.Globalization.NumberStyles.HexNumber);
                            String fillingTimeAux = int_fillingTime.ToString();
                            FILLING_TIME = fillingTimeAux;

                            //incia em 34 (no caso fica 37 - 34)
                            String chargingTime = bytes[37].ToString("X") + bytes[36].ToString("X") + bytes[35].ToString("X") + bytes[34].ToString("X");
                            int int_chargingTime = int.Parse(chargingTime, System.Globalization.NumberStyles.HexNumber);
                            String chargingTimeAux = int_chargingTime.ToString();
                            CHARGIN_TIME = chargingTimeAux;

                            //incia em 38 (no caso fica 41 - 38)
                            String takeoutTime = bytes[41].ToString("X") + bytes[40].ToString("X") + bytes[39].ToString("X") + bytes[38].ToString("X");
                            int int_takeoutTime = int.Parse(takeoutTime, System.Globalization.NumberStyles.HexNumber);
                            String takeoutTimeAux = int_takeoutTime.ToString();
                            TAKE_OUT_TIME = takeoutTimeAux;

                            //incia em 42 (no caso fica 45 - 42)
                            String cycleTime = bytes[45].ToString("X") + bytes[44].ToString("X") + bytes[43].ToString("X") + bytes[42].ToString("X");
                            int int_cycleTime = int.Parse(cycleTime, System.Globalization.NumberStyles.HexNumber);
                            String cycleTimeAux = int_cycleTime.ToString();
                            CYCLE_TIME = cycleTimeAux.Insert(cycleTimeAux.Length - 2, ".");

                            //incia em 46 (no caso fica 49 - 46)
                            String minumumCushionPosition = bytes[49].ToString("X") + bytes[48].ToString("X") + bytes[47].ToString("X") + bytes[46].ToString("X");
                            int int_minumumCushionPosition = int.Parse(minumumCushionPosition, System.Globalization.NumberStyles.HexNumber);
                            String minumumCushionPositionAux = int_minumumCushionPosition.ToString();
                            MINIMUN_CUSHION_POSITION = minumumCushionPositionAux.Insert(minumumCushionPositionAux.Length - 3, ".");

                            //incia em 50 (no caso fica 53 - 50)
                            String cushionPosition = bytes[53].ToString("X") + bytes[52].ToString("X") + bytes[51].ToString("X") + bytes[50].ToString("X");
                            int int_cushionPosition = int.Parse(cushionPosition, System.Globalization.NumberStyles.HexNumber);
                            String cushionPositionAux = int_cushionPosition.ToString();
                            CUSHION_POSITION_ = cushionPositionAux.Insert(cushionPositionAux.Length - 3, ".");

                            //incia em 54 (no caso fica 57 - 54)
                            String dwellChnagePosition = bytes[57].ToString("X") + bytes[56].ToString("X") + bytes[55].ToString("X") + bytes[54].ToString("X");
                            int int_dwellChnagePosition = int.Parse(dwellChnagePosition, System.Globalization.NumberStyles.HexNumber);
                            String dwellChnagePositionAux = int_dwellChnagePosition.ToString();
                            DWELL_CHANGE_POSITION = dwellChnagePositionAux.Insert(dwellChnagePositionAux.Length - 3, ".");

                            //incia em 58 (no caso fica 61 - 58)
                            String injetStartPosition = bytes[57].ToString("X") + bytes[56].ToString("X") + bytes[55].ToString("X") + bytes[54].ToString("X");
                            int int_injetStartPosition = int.Parse(injetStartPosition, System.Globalization.NumberStyles.HexNumber);
                            String injetStartPositionAux = int_injetStartPosition.ToString();
                            INJECTION_START_POSITION = injetStartPositionAux.Insert(injetStartPositionAux.Length - 3, ".");

                            //incia em 62 (no caso fica 65 - 62)
                            String maxInjectPressure = bytes[65].ToString("X") + bytes[64].ToString("X") + bytes[63].ToString("X") + bytes[62].ToString("X");
                            int int_maxInjectPressure = int.Parse(maxInjectPressure, System.Globalization.NumberStyles.HexNumber);
                            String maxInjectPressureAux = int_maxInjectPressure.ToString();
                            MAXIMUM_INJECTION_PRESSURE = maxInjectPressureAux.Insert(maxInjectPressureAux.Length - 3, ".");

                            //incia em 66 (no caso fica 69 - 66)
                            String dwellPressure = bytes[69].ToString("X") + bytes[68].ToString("X") + bytes[67].ToString("X") + bytes[66].ToString("X");
                            int int_dwellPressure = int.Parse(dwellPressure, System.Globalization.NumberStyles.HexNumber);
                            String dwellPressureAux = int_dwellPressure.ToString();
                            DWELL_PRESSURE = dwellPressureAux.Insert(dwellPressureAux.Length - 2, ".");

                            //incia em 70 (no caso fica 73 - 70)
                            String screwRotationSpeed = bytes[73].ToString("X") + bytes[72].ToString("X") + bytes[71].ToString("X") + bytes[70].ToString("X");
                            int int_screwRotationSpeed = int.Parse(screwRotationSpeed, System.Globalization.NumberStyles.HexNumber);
                            String screwRotationSpeedAux = int_screwRotationSpeed.ToString();
                            SCREW_ROTATION_SPEED = screwRotationSpeedAux;

                            //incia em 86 (no caso fica 89 - 86)
                            String temperature_hen = bytes[89].ToString("X") + bytes[88].ToString("X") + bytes[87].ToString("X") + bytes[86].ToString("X");
                            int int_stemperature_hen = int.Parse(temperature_hen, System.Globalization.NumberStyles.HexNumber);
                            String temperature_henAux = int_stemperature_hen.ToString();
                            TEMPERATURE_HEN = temperature_henAux.Insert(dwellPressureAux.Length - 1, ".");

                            //incia em 90 (no caso fica 93 - 90)
                            String temperature_hn = bytes[93].ToString("X") + bytes[92].ToString("X") + bytes[91].ToString("X") + bytes[90].ToString("X");
                            int int_temperature_hn = int.Parse(temperature_hn, System.Globalization.NumberStyles.HexNumber);
                            String temperature_hnAux = int_temperature_hn.ToString();
                            TEMPERATURE_HN = temperature_hnAux.Insert(dwellPressureAux.Length - 1, ".");


                            //incia em 94 (no caso fica 97 - 94)
                            String temperature_h1 = bytes[97].ToString("X") + bytes[96].ToString("X") + bytes[95].ToString("X") + bytes[94].ToString("X");
                            int int_temperature_h1 = int.Parse(temperature_h1, System.Globalization.NumberStyles.HexNumber);
                            String temperature_h1Aux = int_temperature_h1.ToString();
                            TEMPERATURE_H1 = temperature_h1Aux.Insert(temperature_h1Aux.Length - 1, ".");


                            //incia em 98 (no caso fica 101 - 98)
                            String temperature_h2 = bytes[101].ToString("X") + bytes[100].ToString("X") + bytes[99].ToString("X") + bytes[98].ToString("X");
                            int int_temperature_h2 = int.Parse(temperature_h2, System.Globalization.NumberStyles.HexNumber);
                            String temperature_h2Aux = int_temperature_h2.ToString();
                            TEMPERATURE_H2 = temperature_h2Aux.Insert(temperature_h2Aux.Length - 1, ".");

                            //incia em 102 (no caso fica 105 - 102)
                            String temperature_h3 = bytes[105].ToString("X") + bytes[104].ToString("X") + bytes[103].ToString("X") + bytes[102].ToString("X");
                            int int_temperature_h3 = int.Parse(temperature_h3, System.Globalization.NumberStyles.HexNumber);
                            String temperature_h3Aux = int_temperature_h3.ToString();
                            TEMPERATURE_H3 = temperature_h3Aux.Insert(temperature_h3Aux.Length - 1, ".");

                            //incia em 106 (no caso fica 109 - 106)
                            String temperature_h4 = bytes[109].ToString("X") + bytes[108].ToString("X") + bytes[107].ToString("X") + bytes[106].ToString("X");
                            int int_temperature_h4 = int.Parse(temperature_h4, System.Globalization.NumberStyles.HexNumber);
                            String temperature_h4Aux = int_temperature_h4.ToString();
                            TEMPERATURE_H4 = temperature_h4Aux.Insert(temperature_h4Aux.Length - 1, ".");

                            //incia em 110 (no caso fica 113 - 110)
                            String temperature_h5 = bytes[113].ToString("X") + bytes[112].ToString("X") + bytes[111].ToString("X") + bytes[110].ToString("X");
                            int int_temperature_h5 = int.Parse(temperature_h5, System.Globalization.NumberStyles.HexNumber);
                            String temperature_h5Aux = int_temperature_h5.ToString();
                            TEMPERATURE_H5 = temperature_h5Aux.Insert(temperature_h5Aux.Length - 1, ".");

                            //incia em 114 (no caso fica 117 - 114)
                            String temperature_h6 = bytes[117].ToString("X") + bytes[116].ToString("X") + bytes[115].ToString("X") + bytes[114].ToString("X");
                            int int_temperature_h6 = int.Parse(temperature_h6, System.Globalization.NumberStyles.HexNumber);
                            String temperature_h6Aux = int_temperature_h6.ToString();
                            TEMPERATURE_OIL = temperature_h6Aux.Insert(temperature_h6Aux.Length - 1, ".");

                            //incia em 118 (no caso fica 121 - 118)
                            String temperature_hop = bytes[121].ToString("X") + bytes[120].ToString("X") + bytes[119].ToString("X") + bytes[118].ToString("X");
                            int int_temperature_hop = int.Parse(temperature_hop, System.Globalization.NumberStyles.HexNumber);
                            String temperature_hopAux = int_temperature_hop.ToString();
                            TEMPERATURE_HOP = temperature_hopAux.Insert(temperature_hopAux.Length - 1, ".");

                            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ipServidorMap + ":8081/parametrosAtuais/insert");
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";
                            listMaquinas.Items.Add("Enviando dados no pacote de qualidade");

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                string json = "{\"mac\":\"" + ipServer + "\"" + "," +
                      "\"tipo\":\"" + "2" + "\"" + "," +
                      "\"prodShot\":\"" + PRODUCTION_SHOT + "\"" + "," +
                      "\"dwellPressure\":\"" + DWELL_PRESSURE + "\"" + "," +
                      "\"ok_prodShot\":\"" + OK_PRODUCTION_SHOT + "\"" + "," +
                      "\"printShot\":\"" + PRING_SHOT + "\"" + "," +
                      "\"fillingTime\":\"" + FILLING_TIME + "\"" + "," +
                      "\"chargingTime\":\"" + CHARGIN_TIME + "\"" + "," +
                      "\"takeoutTime\":\"" + TAKE_OUT_TIME + "\"" + "," +
                      "\"cycleTime\":\"" + CYCLE_TIME + "\"" + "," +
                      "\"dwellChnagePosition\":\"" + DWELL_CHANGE_POSITION + "\"" + "," +
                      "\"minumumCushionPosition\":\"" + MINIMUN_CUSHION_POSITION + "\"" + "," +
                      "\"cushionPosition\":\"" + CUSHION_POSITION_ + "\"" + "," +
                      "\"injetStartPosition\":\"" + INJECTION_START_POSITION + "\"" + "," +
                      "\"maxInjectPressure\":\"" + MAXIMUM_INJECTION_PRESSURE + "\"" + "," +
                      "\"temperature_hen\":\"" + TEMPERATURE_HEN + "\"" + "," +
                      "\"temperature_hn\":\"" + TEMPERATURE_HN + "\"" + "," +
                      "\"temperature_h1\":\"" + TEMPERATURE_H1 + "\"" + "," +
                      "\"temperature_h2\":\"" + TEMPERATURE_H2 + "\"" + "," +
                      "\"temperature_h3\":\"" + TEMPERATURE_H3 + "\"" + "," +
                      "\"temperature_h4\":\"" + TEMPERATURE_H4 + "\"" + "," +
                      "\"temperature_h5\":\"" + TEMPERATURE_H5 + "\"" + "," +
                      "\"temperature_oil\":\"" + TEMPERATURE_OIL + "\"" + "," +
                      "\"temperature_hop\":\"" + TEMPERATURE_HOP + "\"" + "," +
                      "\"screwRotationSpeed\":\"" + SCREW_ROTATION_SPEED + "\"" + "," +
                      "\"cycleTime\":\"" + CYCLE_TIME + "\""
                      + "}";

                                streamWriter.Write(json);
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                        }
                    }
                    


                    }
            }catch(Exception e){
                Console.WriteLine("Excecao: ", e.ToString());
                client.Close();
            }


        }
        public void conectarOpc()
        {

            try
            {

                fact = new OpcCom.Factory();
                server = new Opc.Da.Server(fact, null);
                server.Url = new Opc.URL("opcda://KeStudio.Opc.LC.1.96");
                server.Connect();

                MessageBox.Show("OPC SERVER CONECTADO COM SUCESSO");
                
            }
            catch (Exception exc)
            {
                MessageBox.Show("ERRO AO SE CONECTAR COM O OPC SERVER");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ColetaInjetoraHaitianListener posto_atual in posto_cadastrado)
            {
                posto_atual.finalizaThreads();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listMaquinas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TelaPrincipal_Load(object sender, EventArgs e)
        {

        }

        public void ativarMaquinas(msws.icDTO icdto_cadastrado)
        {

            foreach (msws.icUpDTO icupdto_atual in icdto_cadastrado.portas)
            {

                string cdUp = icupdto_atual.upDTO.cd_up;
                string cdPtAtual = icupdto_atual.urlConexao;
                string host = icupdto_atual.urlAuxiliar;

                if (cdUp.Contains("_CEP")&&server!=null)
                {
                    
                    //listMaquinas.Items.Add("Codigo: " + cdPtAtual + " Host: " + host);
                    ColetaInjetoraHaitianListener rn_atual = new ColetaInjetoraHaitianListener();
                    rn_atual.inicializaColeta(cdPtAtual, host, icdto_cadastrado, icupdto_atual, server);
                    posto_cadastrado.Add(rn_atual);

                    try
                    {

                        Thread.Sleep(3000);

                    }

                    catch (Exception ex)
                    {
                        log.Info("Erro na chamada do webservice: " + ex);
                        Thread.Sleep(30000);
                    }

                }
                else
                {

                }
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                server.Disconnect();
                conectarOpc();

                foreach (ColetaInjetoraHaitianListener posto_atual in posto_cadastrado)
                {
                    posto_atual.reconectarOpcServer(server,posto_atual.hostInjetora);
                    
                }
                MessageBox.Show("OPC SERVER RECONECTADO COM SUCESSO");


            }
            catch (Exception ex)
            {
                log.Info("Erro na chamada do webservice: " + ex);
                Thread.Sleep(30000);
                MessageBox.Show("Falha ao reconectar o opc");
            }
           
        }

        public void coletautil() {

            ColetaInjetoraHaitianListener rn_atualteste6 = new ColetaInjetoraHaitianListener();
            rn_atualteste6.inicializaColeta("", "47005", null, null, server);


        }
        public void testeOpc()
        {




            ColetaInjetoraHaitianListener rn_atual = new ColetaInjetoraHaitianListener();
            rn_atual.inicializaColeta("", "550/01", null, null, server);
            //posto_cadastrado.Add(rn_atual);

            Thread.Sleep(3000);
            ColetaInjetoraHaitianListener rn_atualteste = new ColetaInjetoraHaitianListener();
            rn_atualteste.inicializaColeta("", "38004", null, null, server);

            //Thread.Sleep(3000);
            ColetaInjetoraHaitianListener rn_atualteste2 = new ColetaInjetoraHaitianListener();
            rn_atualteste2.inicializaColeta("", "130001", null, null, server);
            Thread.Sleep(3000);
            ColetaInjetoraHaitianListener rn_atualteste3= new ColetaInjetoraHaitianListener();
            rn_atualteste3.inicializaColeta("", "80002", null, null, server);
            Thread.Sleep(3000);

            ColetaInjetoraHaitianListener rn_atualteste4 = new ColetaInjetoraHaitianListener();
            rn_atualteste4.inicializaColeta("", "70002", null, null, server);

            Thread.Sleep(3000);
            ColetaInjetoraHaitianListener rn_atualteste5 = new ColetaInjetoraHaitianListener();
            rn_atualteste5.inicializaColeta("", "47004", null, null, server);
            Thread.Sleep(3000);

            //ColetaInjetoraHaitianListener rn_atualteste6 = new ColetaInjetoraHaitianListener();
            //rn_atualteste6.inicializaColeta("", "47005", null, null, server);


            Thread threadSelecionarOp = new Thread(coletautil);
            threadSelecionarOp.Name = "ts";
            threadSelecionarOp.Start();
            //posto_cadastrado.Add(rn_atualteste);

                MessageBox.Show("Inicinaod teste");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtIpServidor_TextChanged(object sender, EventArgs e)
        {

        }
    }
   
   
}
