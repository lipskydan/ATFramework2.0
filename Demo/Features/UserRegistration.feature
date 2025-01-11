Feature: User Registration

Scenario: Successful validation - Full registration flow
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr"
	And Enter text "Clark" to the field "FirstName"
	And Enter text "Smith" to the field "LastName"
	And Enter text "PeterSmith@gmail.com" to the field "Email"
	And Enter text "Peter" to the field "UserName"
	And Enter text "Password" to the field "Password" 
	When Click on the button "Submit"
	Then Success message "User Registered Successfully !!!" is displayed

Scenario: Warning validation - Forget First Name
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr"
	When Click on the button "Submit"
	Then Fail message "This field is required" under the field "FirstName" is displayed

Scenario: Warning validation - Forget Last Name
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr"
	And Enter text "Clark" to the field "FirstName"
	When Click on the button "Submit"
	Then Fail message "This field is required" under the field "LastName" is displayed

Scenario: Warning validation - Forget Email
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr"
	And Enter text "Clark" to the field "FirstName"
	And Enter text "Smith" to the field "LastName"
	When Click on the button "Submit"
	Then Fail message "This field is required" under the field "Email" is displayed

Scenario: Warning validation - Forget UserName
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr"
	And Enter text "Clark" to the field "FirstName"
	And Enter text "Smith" to the field "LastName"
	And Enter text "PeterSmith@gmail.com" to the field "Email"
	When Click on the button "Submit"
	Then Fail message "This field is required" under the field "UserName" is displayed

Scenario: Warning validation - Forget Password
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr"
	And Enter text "Clark" to the field "FirstName"
	And Enter text "Smith" to the field "LastName"
	And Enter text "PeterSmith@gmail.com" to the field "Email"
	And Enter text "Peter" to the field "UserName"
	When Click on the button "Submit"
	Then Fail message "This field is required" under the field "Password" is displayed

Scenario: Warning validation - Not valid Email
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr"
	And Enter text "Clark" to the field "FirstName"
	And Enter text "Smith" to the field "LastName"
	And Enter text "PeterSmith@gmailcom" to the field "Email"
	When Click on the button "Submit"
	Then Fail message "Enter a valid email" under the field "Email" is displayed

	##########################

	Scenario: [FailDemo] Try to find not displayed Success message
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr"
	When Click on the button "Submit"
	Then Success message "User Registered Successfully !!!" is displayed

	# Scenario: [FailDemo] Try to click on not exist button
	# Given Navigate to the start page of the app
	# And Open "Sign In" page
	# And Click on the button "New Registration"
	# And Select Salutation "Mr"
	# And Enter text "Clark" to the field "FirstName"
	# And Enter text "Smith" to the field "LastName"
	# And Enter text "PeterSmith@gmail.com" to the field "Email"
	# And Enter text "Peter" to the field "UserName"
	# And Enter text "Password" to the field "Password" 
	# When Click on the button "Submit1"
	# Then Success message "User Registered Successfully !!!" is displayed

	# Scenario: [FailDemo] Try to input text in not exist field
	# Given Navigate to the start page of the app
	# And Open "Sign In" page
	# And Click on the button "New Registration"
	# And Select Salutation "Mr"
	# And Enter text "Clark" to the field "FullName"
	# And Enter text "Smith" to the field "LastName"
	# And Enter text "PeterSmith@gmail.com" to the field "Email"
	# And Enter text "Peter" to the field "UserName"
	# And Enter text "Password" to the field "Password" 
	# When Click on the button "Submit"
	# Then Success message "User Registered Successfully !!!" is displayed
