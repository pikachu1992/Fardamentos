﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Fardamentos
{
    public partial class FindInvent : Form
    {
        public class Find
        {
            public int TipoEquipamento;
            public int Equipamento;
            public int Tamanho;

        }

        public class FillResultadosEventArgs:EventArgs
        {
            public Find result;
        }

        public delegate void FillResultadosEventHandler(object sender, FillResultadosEventArgs e);

        public event FillResultadosEventHandler FillResultados;

        public FindInvent()
        {
            InitializeComponent();
            TipoEquipamento();
            Equipamento();
            Tamanho();
        }

        private void btnFindInvent_Click(object sender, EventArgs e)
        {
            Find result = new Find();

            result.TipoEquipamento = Convert.ToInt32(cboxTipoEquipamento.SelectedValue);
            result.Equipamento = Convert.ToInt32(cboxEquipamento.SelectedValue);
            result.Tamanho = Convert.ToInt32(cboxTamanho.SelectedValue);

            FillResultadosEventArgs Args = new FillResultadosEventArgs();

            Args.result = result;
            
            FillResultados(this, Args);

            Hide();
               
        }
      

        public void TipoEquipamento()
        {
            MySqlConnection conn = new MySqlConnection(Database.Database.ConnectionString);

            try
            {
                conn.Open();

                string sqlTipoEquipamento = "SELECT `tiposfardamento`.`id`, `tiposfardamento`.`tipo` FROM inventario LEFT JOIN equipamento ON inventario.equipamento = equipamento.id LEFT JOIN tiposfardamento on equipamento.tipo = tiposfardamento.id group by `tiposfardamento`.`id`";

                MySqlCommand sqlCmd = new MySqlCommand(sqlTipoEquipamento, conn);

                MySqlDataAdapter mysqlDs = new MySqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                mysqlDs.Fill(ds);
                cboxTipoEquipamento.DataSource = ds.Tables[0];
                cboxTipoEquipamento.ValueMember = "id";
                cboxTipoEquipamento.DisplayMember = "tipo";

            }
            catch (Exception crap)
            {
                MessageBox.Show(crap.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        public void Equipamento()
        {
            MySqlConnection conn = new MySqlConnection(Database.Database.ConnectionString);

            try
            {
                conn.Open();

                string sqlEquipamento = "SELECT `equipamento`.`id`, `equipamento`.`nome` FROM inventario LEFT JOIN equipamento on inventario.equipamento = equipamento.id WHERE tipo = @tipo group by `equipamento`.`id`";

                MySqlCommand sqlCmd = new MySqlCommand(sqlEquipamento, conn);
                sqlCmd.Parameters.AddWithValue("@tipo", cboxTipoEquipamento.SelectedValue);

                MySqlDataAdapter mysqlDs = new MySqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                mysqlDs.Fill(ds);
                cboxEquipamento.DataSource = ds.Tables[0];
                cboxEquipamento.ValueMember = "id";
                cboxEquipamento.DisplayMember = "nome";

            }
            catch (Exception crap)
            {
                MessageBox.Show(crap.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        public void Tamanho()
        {
            MySqlConnection conn = new MySqlConnection(Database.Database.ConnectionString);

            try
            {
                conn.Open();

                string sqlTamanho = "SELECT `tamanhos`.`id`, `tamanhos`.`tam` FROM inventario LEFT JOIN equipamento ON inventario.equipamento = equipamento.id LEFT JOIN tiposfardamento on equipamento.tipo = tiposfardamento.id LEFT JOIN tamanhos on inventario.tamanho = tamanhos.id WHERE inventario.equipamento = @equipamento and equipamento.tipo = @tipo";

                MySqlCommand sqlCmd = new MySqlCommand(sqlTamanho, conn);
                sqlCmd.Parameters.AddWithValue("@tipo", cboxTipoEquipamento.SelectedValue);
                sqlCmd.Parameters.AddWithValue("@equipamento", cboxEquipamento.SelectedValue);

                MySqlDataAdapter mysqlDs = new MySqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                mysqlDs.Fill(ds);
                cboxTamanho.DataSource = ds.Tables[0];
                cboxTamanho.ValueMember = "id";
                cboxTamanho.DisplayMember = "tam";

            }
            catch (Exception crap)
            {
                MessageBox.Show(crap.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void cboxTipoEquipamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            Equipamento();
            Tamanho();
        }
    }
}
