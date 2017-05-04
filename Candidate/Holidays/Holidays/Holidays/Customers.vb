Imports System.IO
Imports System.Net.Mail
Public Class Customers
    Dim Last As Integer
    Private Structure Customer
        Public SessionID As String
        Public FirstName As String
        Public LastName As String                'Creating the structure that will hold the data.
        Public Email As String
        Public PhoneNumber As String
    End Structure
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        'Declaring varibales and assigning values if appropriate.
        Dim SessionID As String
        Dim Firstname As String = txtFirstName.Text
        Dim LastName As String = txtLastName.Text
        Dim Email As String = txtEmail.Text
        Dim PhoneNumber As String = txtPhoneNumber.Text
        Dim Validated As Boolean = True


        'this is a simple length check, Using the .length method to check the length of the phone number 
        'and then comparing it to 11, if it is not 11 then the phone number is invalid and an error message displayed
        ' The validation variable will become false
        If PhoneNumber.Length <> 11 Then
            Validated = False
            MsgBox("Please ensure that the phone number is 11 numbers long")
        End If


        'Here i am using the IsNumeric Function which returns a boolean value of whether or not the data passed is numeric.
        'i am checking if this is false, meaning it is not numeric, if this is so an error message is displayed. The validation variable will become false
        If IsNumeric(PhoneNumber) = False Then
            Validated = False
            MsgBox("Please ensure the phone number only encludes numbers and not letters")
        End If

        'these are four simple presence checks, if the variable has no data within them, meaning they are empty, then 
        'the user is told to put data in them, The validation variable will become false
        If Firstname = "" Or LastName = "" Or Email = "" Or PhoneNumber = "" Then
            Validated = False
            MsgBox("Please make sure every field is full and not empty")
        End If

        'Here i am utilising the try catch method, it will try to pass the value of email into a variable which is defined as an email
        'if this doesnt work it is due to the Email not being of the correct format, this is called a format check.
        'If it fails then when it is caught then an error message is displayed. The validation variable will become false
        Try
            Dim Test As New MailAddress(Email)
        Catch

            Validated = False
            MsgBox("Please make sure the email is correct and in the right format")

        End Try

        'If none of the above validations changed "Validated" into False then this will be run, if they have then it will be skipped.
        If Validated = True Then


            'Here i am checking if there are any items in the combobox then a value of "1000" will be given to sessionID
            'This is the value the first entry will take.
            If cmbSearch.Items.Count = 0 Then
                SessionID = 1000

            Else
                'Else if there are values in the combobox then i wil get the last value from the combo box and add 1 to it
                'this is essentially incrementing the id
                SessionID = Val((cmbSearch.GetItemText(cmbSearch.Items(cmbSearch.Items.Count - 1)))) + 1
            End If

            'Here i am adding the new ID to the combobox
            cmbSearch.Items.Add(SessionID)


            'Here i am going to be writing the data into a text file, created earlier
            Dim CustomerData As New Customer
            'This code is assigning the values to the structure above.
            Dim sw As New System.IO.StreamWriter("Customer.txt", True)
            CustomerData.SessionID = LSet(SessionID, 50)
            CustomerData.FirstName = LSet(Firstname, 50)
            CustomerData.LastName = LSet(LastName, 50)
            CustomerData.Email = LSet(Email, 50)                      'Filling the structure with data.
            CustomerData.PhoneNumber = LSet(PhoneNumber, 50)
            'this is actually writing the values from the structure into the text file in a certain order.
            sw.WriteLine(CustomerData.SessionID & CustomerData.FirstName & CustomerData.LastName & CustomerData.Email & CustomerData.PhoneNumber)
            sw.Close()                                                                  'Always need to close afterwards
            MsgBox("File Saved!")
        End If



    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'Here i am reading the text file
        Dim CustomerRead() As String = File.ReadAllLines("Customer.txt")

        'This loop will loop through each line of the text file
        For i = 0 To UBound(CustomerRead)
            'If the ID in the text file matches the searchID then the values from the text file will be put into the textboxes 
            If Trim(Mid(CustomerRead(i), 1, 50)) = cmbSearch.Text Then
                txtSessionID.Text = Trim(Mid(CustomerRead(i), 1, 50))
                txtFirstName.Text = Trim(Mid(CustomerRead(i), 51, 50))
                txtLastName.Text = Trim(Mid(CustomerRead(i), 101, 50))
                txtEmail.Text = Trim(Mid(CustomerRead(i), 151, 50))
                txtPhoneNumber.Text = Trim(Mid(CustomerRead(i), 201, 50))

            End If

        Next i
    End Sub

    Private Sub btnSwapFormHolidays_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSwapFormHolidays.Click

        'this button here will hide this form and show the other form

        Dim Form As Form
        Form = New Holidays
        Form.Show()
        Me.Hide()
    End Sub

    Private Sub Customers_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        If Dir$("Customer.txt") = "" Then
            Dim sw As New StreamWriter("Customer.txt", True)    'This makes sure there is actually a database to enter/read data. If not, it creates a new blank one.
            sw.WriteLine("")
            sw.Close()
            MsgBox("A new file has been created", vbExclamation, "Warning!")
        End If


        'This reads the text file
        Dim ComboAdd() As String = File.ReadAllLines("customer.txt")

        'This will read line by line the text file and add each value of the ID into the combobox
        For i = 0 To UBound(ComboAdd)

            cmbSearch.Items.Add(Trim(Mid(ComboAdd(i), 1, 50)))

        Next i



    End Sub
End Class