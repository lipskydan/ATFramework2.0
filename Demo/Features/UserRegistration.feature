Feature: UserRegistration

Scenario: Successful Validation
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr."
	And Enter FirstName "Clark"
	And Enter LastName "Smith"
	And Enter Email "PeterSmith@gmail.com"
	And Enter UserName "Peter"
	And Enter Password "Password"
	When Click on the button "Submit"
	Then Success message "User Registered Successfully !!!" is displayed

Scenario: Fail Validation - Forget First Name
	Given Navigate to the start page of the app
	And Open "Sign In" page
	And Click on the button "New Registration"
	And Select Salutation "Mr."
	When Click on the button "Submit"
	Then Fail message "This field is required" under the field "FirstName" is displayed
