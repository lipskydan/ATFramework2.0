Feature: UserRegistration

Scenario: Successful Validation
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation
	And Enter FirstName
	And Enter LastName
	And Enter ValidEmail
	And Enter UserName
	And Enter Password
	When Click on the button "Submit"
	Then Success message "User Registered Successfully !!!" is displayed
