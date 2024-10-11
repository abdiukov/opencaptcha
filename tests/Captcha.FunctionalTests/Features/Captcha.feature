Feature: Captcha
In order to validate captcha generation
As an API client
I want to send different captcha requests and assure the image is generated

    Scenario Outline: Send captcha requests
        Given I have a captcha request with following parameters:
          | Text   | Width   | Height   | Difficulty   |
          | <Text> | <Width> | <Height> | <Difficulty> |
        When I send the request to the Create endpoint of the CaptchaController
        Then I expect a captcha image to be returned with the following attributes:
          | Width           | Height           |
          | <ExpectedWidth> | <ExpectedHeight> |

        Examples:
          | Text         | Width | Height | Difficulty  | ExpectedWidth | ExpectedHeight |
          | مرحبًا       |       |        |             | 400           | 100            |
          | 你好         |       |        |             | 400           | 100            |
          | こんにちは    |       |        |             | 400           | 100            |
          | 안녕하세요     |       |        |             | 400           | 100            |
          | Здравствуйте |       |        |             | 400           | 100            |
          | Bonjour      |       |        |             | 400           | 100            |
          | Guten Tag    |       |        |             | 400           | 100            |
          | Selam        |       |        |             | 400           | 100            |
          | Γεια σας     |       |        |             | 400           | 100            |
          | Lorem        |       |        |             | 400           | 100            |
          | Ipsum        |       | 200    |             | 400           | 200            |
          | helloworld   | 200   |        | Easy        | 200           | 100            |
          | bar          | 300   | 300    | Medium      | 300           | 300            |
          | foo          | 400   | 400    | Hard        | 400           | 400            |
          | Ciao         | 200   |        | Easy        | 200           | 100            |
          | Olá          | 300   | 300    | Challenging | 300           | 300            |
          | สวัสดี         | 400   | 400    | Hard        | 400           | 400            |

    Scenario: Captcha should not have any black borders
        Given I have a captcha request with following parameters:
          | Text    | Width | Height | Difficulty |
          | Bonjour |       |        | Easy       |
        When I send the request to the Create endpoint of the CaptchaController
        Then I expect a captcha image to be returned with the following attributes:
          | Width | Height |
          | 400   | 100    |
        Then I expect a captcha image to be returned without any black borders
