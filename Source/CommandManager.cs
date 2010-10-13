// ------------------------------------------------------------
// Writer, WYSIWYG editor for HTML
// Copyright (c) 2002-2003 Lutz Roeder. All rights reserved.
// http://www.lutzroeder.com/dotnet
// ------------------------------------------------------------
namespace Writer
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Windows.Forms;
	using Writer.Forms;

	internal class CommandManager
	{
		private Hashtable itemTable = new Hashtable();
		private ArrayList targets = new ArrayList();
		private ArrayList states = new ArrayList();

		public void Add(string commandName, CommandBarControl control)
		{
			control.Click += new EventHandler(this.CommandBarControl_Click);

			CommandState state = this.GetCommandState(commandName, control);
			state.AddItem(control);

			this.itemTable[control] = commandName;
		}

		public void AddTarget(ICommandTarget target)
		{
			this.targets.Insert(0, target);
		}

		public void RemoveTarget(ICommandTarget target)
		{
			this.targets.Remove(target);
		}

		public void QueryStatus()
		{
			foreach (CommandState commandState in this.states)
			{
				this.QueryStatus(commandState);
			}
		}

		private CommandState GetCommandState(string commandName, CommandBarControl control)
		{
			foreach (CommandState commandState in this.states)
			{
				if (commandName == commandState.CommandName)
				{
					return commandState;
				}
			}

			CommandBarButton button = control as CommandBarButton;
			if (button != null)
			{
				CommandButtonState commandState = new CommandButtonState(commandName);
				this.states.Add(commandState);
				return commandState;
			}

			CommandBarCheckBox checkBox = control as CommandBarCheckBox;
			if (checkBox != null)
			{
				CommandCheckBoxState commandState = new CommandCheckBoxState(commandName);
				this.states.Add(commandState);
				return commandState;
			}

			CommandBarComboBox comboBox = control as CommandBarComboBox;
			if (comboBox != null)
			{
				CommandComboBoxState commandState = new CommandComboBoxState(commandName);
				this.states.Add(commandState);
				return commandState;
			}

			throw new NotSupportedException();
		}

		private void Execute(CommandState commandState)
		{
			Debug.WriteLine("Execute \'" + commandState.CommandName + "\'.");
			
			if (commandState.IsEnabled)
			{
				foreach (ICommandTarget commandTarget in this.targets)
				{
					// commandTarget.QueryStatus(commandState);
					if (commandTarget.Execute(commandState))
					{
						this.QueryStatus();
						return;
					}
				}
			}
		}
	
		private void QueryStatus(CommandState commandState)
		{
			foreach (ICommandTarget commandTarget in this.targets)
			{
				if (commandTarget.QueryStatus(commandState))
				{
					return;
				}
			}

			commandState.IsEnabled = false;
		}

		private void CommandBarControl_Click(object sender, EventArgs e)
		{
			CommandBarControl control = sender as CommandBarControl;
			if (control != null)
			{
				string commandName = (string)this.itemTable[control];
				CommandState commandState = this.GetCommandState(commandName, control);
				commandState.ActiveItem = control;
				this.Execute(commandState);
			}
		}
	}

	internal class CommandState
	{
		private string commandName;
		private ArrayList items = new ArrayList();
		private CommandBarItem activeItem = null;

		public CommandState(string commandName)
		{
			this.commandName = commandName;
		}

		internal void AddItem(CommandBarItem item)
		{
			this.items.Add(item);
		}

		internal void RemoveItem(CommandBarItem item)
		{
			this.items.Remove(item);
		}

		internal CommandBarItem ActiveItem
		{
			set { this.activeItem = value; }
			get { return this.activeItem; }
		}

		internal IEnumerable Items
		{
			get { return this.items; }
		}

		public string CommandName
		{
			get { return this.commandName; }
		}

		public bool IsVisible
		{
			set
			{
				foreach (CommandBarItem item in this.items)
				{
					item.Visible = value;
				}
			}

			get { return this.ActiveItem.Visible; }
		}

		public bool IsEnabled
		{
			set
			{
				foreach (CommandBarItem item in this.items)
				{
					item.Enabled = value;
				}
			}

			get { return this.ActiveItem.Enabled; }
		}

		public string Text
		{
			set
			{
				foreach (CommandBarItem item in this.items)
				{
					item.Text = value;
				}
			}

			get { return this.ActiveItem.Text; }
		}
	}

	internal class CommandButtonState : CommandState
	{
		public CommandButtonState(string commandName) : base(commandName)
		{
		}
	}

	internal class CommandCheckBoxState : CommandState
	{
		public CommandCheckBoxState(string commandName) : base(commandName)
		{
		}

		public bool IsChecked
		{
			set
			{
				foreach (CommandBarCheckBox checkBox in this.Items)
				{
					checkBox.IsChecked = value;
				}
			}

			get { return (this.ActiveItem as CommandBarCheckBox).IsChecked; }
		}
	}

	internal class CommandComboBoxState : CommandState
	{
		public CommandComboBoxState(string commandName) : base(commandName)
		{
		}

		public string Value
		{
			set
			{
				foreach (CommandBarComboBox comboBox in this.Items)
				{
					comboBox.Value = value;
				}
			}

			get	{ return (this.ActiveItem as CommandBarComboBox).Value; }
		}
	}
}