Feature: Todo Management

Background:
  Given I am a new user
  And I register with valid credentials
  And I am logged in

Scenario: Add a new todo
  When I open the todo page
  And I add todo "Buy milk"
  Then the todo list should contain "Buy milk"
