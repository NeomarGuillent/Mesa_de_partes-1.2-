using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesa_de_partes_1._2_
{
    internal class Program
    {
        static int max_sol = 100;
        static string[] nombres_sol = new string[max_sol];
        static string[] dni_sol = new string[max_sol];
        static string[] telefono_sol = new string[max_sol];
        static string[] correo_sol = new string[max_sol];
        static string[] postulante_sol = new string[max_sol];
        static string[] tipo_sol = new string[max_sol];
        static string[] asunto_sol = new string[max_sol];
        static string[] tipos_doc = { "Carta de presentación", "Justificación por enfermedad" };
        static string[] codigo_sol = new string[max_sol];
        static string[] num_id_sol = new string[max_sol];
        static string[] tipo_id_sol = new string[max_sol];
        static string carpeta_recibidos = "recibidos";
        static string[] dominios_correo = { "gmail.com", "hotmail.com", "outlook.com", "yahoo.com" };
        static int con_sol = 0;


        static public void titulo()
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("              MESA DE PARTES DIGITAL             ");
            Console.WriteLine("         Colegio Italiano Antonio Raimondi       ");
            Console.WriteLine("-------------------------------------------------");
        }

        static public string pedir_texto(string mensaje)
        {
            
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(texto))
                    return texto;
                Console.WriteLine("ERROR 002: ESTE CAMPO ES OBLIGATORIO.");
            }
        }
        static public string pedir_telefono()
        {
            
            while (true)
            {
                Console.Write("Teléfono (9 dígitos, empieza con 9): ");
                string telefono = Console.ReadLine()?.Trim();
                if (telefono != null && telefono.Length == 9 && solo_digitos(telefono) && telefono[0] == '9')
                    return telefono;
                Console.WriteLine("ERROR 008: EL TELÉFONO DEBE TENER 9 DÍGITOS Y EMPEZAR POR 9.");
            }
        }
        static public bool solo_digitos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return false;
            foreach (char c in texto)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        static public string pedir_tipo_id()
        {
            Console.WriteLine("Documento de identidad:");
            Console.WriteLine("[1] DNI");
            Console.WriteLine("[2] Carné de extranjería");
            while (true)
            {
                Console.Write("Elegir opción: ");
                string texto = Console.ReadLine()?.Trim();
                if (texto == "1")
                    return "DNI";
                if (texto == "2")
                    return "CE";
                Console.WriteLine("ERROR 009: ELEGIR 1 (DNI) O 2 (CARNÉ DE EXTRANJERÍA).");
            }
        }

        static public string pedir_numero_id(string tipo_id)
        {
            
            int largo = (tipo_id == "DNI") ? 8 : 9;
            while (true)
            {
                Console.Write($"{tipo_id} ({largo} dígitos): ");
                string numero = Console.ReadLine()?.Trim();
                if (numero != null && numero.Length == largo && solo_digitos(numero))
                    return numero;
                Console.WriteLine($"ERROR 003: EL {tipo_id} DEBE TENER EXACTAMENTE {largo} DÍGITOS.");
            }
        }


        static public string pedir_tipo()
        {
            Console.WriteLine("Tipo de documento:");
            for (int i = 0; i < tipos_doc.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {tipos_doc[i]}");
            }
            while (true)
            {
                Console.Write("Elegir tipo: ");
                string texto = Console.ReadLine()?.Trim();
                if (int.TryParse(texto, out int opcion) && opcion >= 1 && opcion <= tipos_doc.Length)
                    return tipos_doc[opcion - 1];
                Console.WriteLine($"ERROR 004: ELEGIR UN NÚMERO ENTRE 1 Y {tipos_doc.Length}.");
            }
        }

        static public string pedir_correo()
        {
            
            while (true)
            {
                Console.Write("Correo electrónico (ej: nombre@gmail.com): ");
                string correo = Console.ReadLine()?.Trim().ToLower();
                if (correo != null && !correo.Contains(" "))
                {
                    int pos = correo.IndexOf('@');
                    
                    if (pos > 0 && pos == correo.LastIndexOf('@'))
                    {
                        string dominio = correo.Substring(pos + 1);
                        foreach (string valido in dominios_correo)
                        {
                            if (dominio == valido)
                                return correo;
                        }
                    }
                }
                Console.WriteLine("ERROR 010: CORREO INVÁLIDO. DOMINIOS PERMITIDOS: " + string.Join(", ", dominios_correo) + ".");
            }
        }
        static public void guardar_cargo(int i)
        {
            Directory.CreateDirectory(carpeta_recibidos);   
            string ruta = Path.Combine(carpeta_recibidos, codigo_sol[i] + ".txt");
            using (StreamWriter escribir = new StreamWriter(ruta))
            {
                escribir.WriteLine("=================================================");
                escribir.WriteLine("              CARGO DE RECEPCIÓN                 ");
                escribir.WriteLine("         Colegio Italiano Antonio Raimondi       ");
                escribir.WriteLine("=================================================");
                escribir.WriteLine($"Código       : {codigo_sol[i]}");
                escribir.WriteLine($"Fecha y hora : {DateTime.Now:dd/MM/yyyy HH:mm}");
                escribir.WriteLine("-------------------------------------------------");
                escribir.WriteLine($"Postulante   : {postulante_sol[i]}");
                escribir.WriteLine($"Documento    : {tipo_sol[i]}");
                escribir.WriteLine($"Asunto       : {asunto_sol[i]}");
                escribir.WriteLine("-------------------------------------------------");
                escribir.WriteLine($"Presentado por: {nombres_sol[i]} ({tipo_id_sol[i]}: {num_id_sol[i]})");
                escribir.WriteLine($"Teléfono     : {telefono_sol[i]}");
                escribir.WriteLine($"Correo       : {correo_sol[i]}");
                escribir.WriteLine("=================================================");
            }
            Console.WriteLine("Cargo guardado en: " + Path.GetFullPath(ruta));
        }
        static public void registrar_solicitud()
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("****REGISTRAR SOLICITUD****");
            Console.WriteLine("-------------------------------------------------");
            if (con_sol >= max_sol)
            {
                Console.WriteLine("ERROR 001: CAPACIDAD MAXIMA ALCANZADA.");
                return;
            }
            nombres_sol[con_sol] = pedir_texto("Nombres y apellidos: ");
            tipo_id_sol[con_sol] = pedir_tipo_id();
            num_id_sol[con_sol] = pedir_numero_id(tipo_id_sol[con_sol]);
            telefono_sol[con_sol] = pedir_telefono();
            correo_sol[con_sol] = pedir_correo();
            postulante_sol[con_sol] = pedir_texto("Nombre del postulante: ");
            tipo_sol[con_sol] = pedir_tipo();
            asunto_sol[con_sol] = pedir_texto("Asunto: ");
            codigo_sol[con_sol] = "C-" + (con_sol + 1).ToString("000");
            mostrar_cargo(con_sol);
            guardar_cargo(con_sol);
            con_sol++;
            Console.WriteLine("Solicitud recibida correctamente.");
        }

        static public void mostrar()
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("****SOLICITUDES RECIBIDAS****");
            Console.WriteLine("-------------------------------------------------");
            if (con_sol == 0)
            {
                Console.WriteLine("ERROR 005: NO HAY SOLICITUDES POR MOSTRAR.");
                return;
            }
            for (int i = 0; i < con_sol; i++)
            {
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine($"Código: {codigo_sol[i]}");
                Console.WriteLine($"Nombre: {nombres_sol[i]} | {tipo_id_sol[i]}: {num_id_sol[i]}");
                Console.WriteLine($"Teléfono: {telefono_sol[i]} | Correo: {correo_sol[i]}");
                Console.WriteLine($"Postulante: {postulante_sol[i]}");
                Console.WriteLine($"Documento: {tipo_sol[i]}");
                Console.WriteLine($"Asunto: {asunto_sol[i]}");
                Console.WriteLine("-------------------------------------------------");
            }
            Console.WriteLine("Total: " + con_sol);
        }

        static public void mostrar_cargo(int i)
        {
            Console.WriteLine();
            Console.WriteLine("=================================================");
            Console.WriteLine("              CARGO DE RECEPCIÓN                 ");
            Console.WriteLine("         Colegio Italiano Antonio Raimondi       ");
            Console.WriteLine("=================================================");
            Console.WriteLine($"Código       : {codigo_sol[i]}");
            Console.WriteLine($"Fecha y hora : {DateTime.Now:dd/MM/yyyy HH:mm}");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Postulante   : {postulante_sol[i]}");
            Console.WriteLine($"Documento    : {tipo_sol[i]}");
            Console.WriteLine($"Asunto       : {asunto_sol[i]}");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"Presentado por: {nombres_sol[i]} ({tipo_id_sol[i]}: {num_id_sol[i]})");
            Console.WriteLine($"Teléfono     : {telefono_sol[i]}");
            Console.WriteLine($"Correo       : {correo_sol[i]}");
            Console.WriteLine("=================================================");
        }
        static void Main(string[] args)
        {
           

            int opc = 0;
            while (opc != 3)
            {
                Console.Clear();
                titulo();
                Console.WriteLine("[1] Registrar solicitud");
                Console.WriteLine("[2] Mostrar solicitudes recibidas");
                Console.WriteLine("[3] Salir");
                Console.Write("Ingrese una opción: ");
                if (!int.TryParse(Console.ReadLine(), out opc))
                {
                    Console.WriteLine("ERROR 006: OPCIÓN INVÁLIDA. DEBE INGRESAR UN NÚMERO.");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    continue;
                }
                switch (opc)
                {
                    case 1:
                        registrar_solicitud();
                        break;
                    case 2:
                        mostrar();
                        break;
                    case 3:
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("ERROR 007: OPCIÓN INVÁLIDA. DEBE INGRESAR UN NÚMERO ENTRE 1 Y 3.");
                        break;
                }
                Console.ReadKey();
            }
        }
    }
}
