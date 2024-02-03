Feature: UserRegistration

Scenario: Successful Validation
	Given Navigate to the start page of the app
	And click on the button "Sign In Portal"
	And click on the button "New Registration"
	And select Salutation
	And enter FirstName
	And enter LastName
	And enter ValidEmail
	And enter UserName
	And enter Password
	When click on the button "Submit"
	Then Success message "User Registered Successfully !!!" is displayed
