Feature: Captcha
    In order to validate captcha generation
    As an API client
    I want to send different captcha requests and assure the image is generated

Scenario Outline: Send captcha requests
    Given I have a captcha request with following parameters:
        | Text   | Width | Height | Difficulty |
        | <Text> | <Width> | <Height> | <Difficulty> |
    When I send the request to the Create endpoint of the CaptchaController
    Then I expect a captcha image to be returned with the following attributes:
        | Width | Height |
        | <ExpectedWidth> | <ExpectedHeight> |

Examples:
    | Text       | Width | Height | Difficulty     | ExpectedWidth | ExpectedHeight |
    | Lorem      |       |        |                | 400           | 100            |
    | Ipsum      |       | 200    |                | 400           | 200            |
    | helloworld | 200   |        | Easy           | 200           | 100            |
    | bar        | 300   | 300    | Medium         | 300           | 300            |
    | foo        | 400   | 400    | Hard           | 400           | 400            |
