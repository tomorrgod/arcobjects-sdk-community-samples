'Copyright 2019 Esri

'Licensed under the Apache License, Version 2.0 (the "License");
'you may not use this file except in compliance with the License.
'You may obtain a copy of the License at

'    http://www.apache.org/licenses/LICENSE-2.0

'Unless required by applicable law or agreed to in writing, software
'distributed under the License is distributed on an "AS IS" BASIS,
'WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

'See the License for the specific language governing permissions and
'limitations under the License.
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Namespace GlobeGraphicsToolbar
	Partial Public Class TextForm
		Inherits Form
		Private _inputText As String

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub textBox_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles textBox1.KeyUp
			If e.KeyCode = Keys.Enter Then
				_inputText = Me.textBox1.Text
				Me.Close()
			ElseIf e.KeyCode = Keys.Escape Then
				Me.Close()
			End If
		End Sub

		Public ReadOnly Property InputText() As String
			Get
				Return _inputText
			End Get
		End Property
	End Class
End Namespace