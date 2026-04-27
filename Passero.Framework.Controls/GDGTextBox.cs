using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    /// <summary>
    /// TextBox personalizzato con proprietà per la gestione della visibilità e dell'editabilità
    /// in base a liste di utenti o gruppi
    /// </summary>
    [DefaultBindingProperty("Text")]
    public class GDGTextBox : Wisej.Web.TextBox
    {
        private List<string> _visibilityAllowedUsers = new List<string>();
        private List<string> _editabilityAllowedUsers = new List<string>();
        private bool _enableVisibilityCheck = false;
        private bool _enableEditabilityCheck = false;
        private string _businessObjectName = string.Empty;
        private string _businessSystemName = string.Empty;

        /// <summary>
        /// Nome dell'oggetto di business associato al controllo
        /// </summary>
        [Category("Business Namespace")]
        [Description("Nome dell'oggetto di business associato al controllo")]
        public string BusinessObjectName
        {
            get { return _businessObjectName; }
            set { _businessObjectName = value ?? string.Empty; }
        }
        /// <summary>
        /// Nome del Business System associato al controllo
        /// </summary>
        [Category("Business Namespace")]
        [Description("Nome del Business System associato al controllo")]
        public string BusinessSystemName
        {
            get { return _businessSystemName; }
            set { _businessSystemName = value ?? string.Empty; }
            
        }



        /// <summary>
        /// Lista di utenti o gruppi a cui è consentito vedere il controllo
        /// </summary>
        [Category("Control Security")]
        [Description("Lista di utenti o gruppi a cui è consentito vedere il controllo")]
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor")]
        public List<string> VisibilityAllowedUsers
        {
            get { return _visibilityAllowedUsers; }
            set
            {
                _visibilityAllowedUsers = value ?? new List<string>();
                if (_enableVisibilityCheck)
                    ApplySecurityRules();
            }
        }

        /// <summary>
        /// Lista di utenti o gruppi a cui è consentito modificare il controllo
        /// </summary>
        [Category("Control Security")]
        [Description("Lista di utenti o gruppi a cui è consentito modificare il controllo")]
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor")]
        public List<string> EditabilityAllowedUsers
        {
            get { return _editabilityAllowedUsers; }
            set
            {
                _editabilityAllowedUsers = value ?? new List<string>();
                if (_enableEditabilityCheck)
                    ApplySecurityRules();
            }
        }

        /// <summary>
        /// Abilita o disabilita il controllo di visibilità basato sulla lista utenti
        /// </summary>
        [Category("Control Security")]
        [Description("Abilita o disabilita il controllo di visibilità basato sulla lista utenti")]
        [DefaultValue(false)]
        public bool EnableVisibilityCheck
        {
            get { return _enableVisibilityCheck; }
            set
            {
                _enableVisibilityCheck = value;
                ApplySecurityRules();
            }
        }

        /// <summary>
        /// Abilita o disabilita il controllo di editabilità basato sulla lista utenti
        /// </summary>
        [Category("Control Security")]
        [Description("Abilita o disabilita il controllo di editabilità basato sulla lista utenti")]
        [DefaultValue(false)]
        public bool EnableEditabilityCheck
        {
            get { return _enableEditabilityCheck; }
            set
            {
                _enableEditabilityCheck = value;
                ApplySecurityRules();
            }
        }

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="GDGTextBox"/>
        /// </summary>
        public GDGTextBox()
        {
            // Inizializzazione dei valori predefiniti

        }

        /// <summary>
        /// Applica le regole di sicurezza in base all'utente corrente utilizzando la
        /// classe di utilità SecurityUtilities
        /// </summary>
        public void ApplySecurityRules()
        {
            if (_enableVisibilityCheck || _enableEditabilityCheck)
            {
                //Passero.Framework.SecurityUtilities.ApplySecurityRules(
                //    this,
                //    _enableVisibilityCheck ? _visibilityAllowedUsers : null,
                //    _enableEditabilityCheck ? _editabilityAllowedUsers : null
                //);
            }
        }
    }

    
}