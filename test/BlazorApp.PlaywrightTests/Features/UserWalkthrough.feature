Feature: User Walkthrough (Registration + Login)

  Scenario: Register new user, login, verify home page, then logout
    Given I navigate to the home page
    And the home page shows "Hello, world!"
    When I open the login page
    And I enter invalid credentials
    Then I see login error "Error: Invalid login attempt."
    When I open the register page
    And I register with valid credentials
    Then I see register confirmation page
    When I click the confirmation link
    And I login with the registered credentials
    Then I am on the home page after successful login
    When I click logout from the home page
    Then I am back on the home page
