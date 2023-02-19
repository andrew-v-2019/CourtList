   ### API Endpoint запущен по адресу https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo
 
  * В ходе реализации появилась проблема: Яндекс генерирует ошибку "JobResponseTooLong", если JSON слишком большой
  * Укорачивание строк, путём удаления лишних пробелов, точек, ненужных символов, сокращения слов, ощутимого результата не даёт
  * В документаций ничего нет по этому поводу, поэтому пришлось реализовать постраничный вывод
  
   * Структура запроса: https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo?login={login}&password={password}&page={page} 

   * Примеры запросов:
	  
	  - https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo - вернёт ошибку "Доступ запрещён"
	  
	   - https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo?login=test&password=testpass - вернёт первую страницу
	  
	   - https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo?login=test&password=testpass&page=0    - вернёт первую страницу
	  
	   - https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo?login=test&password=testpass&page=1    - вернёт первую страницу
	  
	   - https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo?login=test&password=testpass&page=2    - вернёт вторую страницу
	  
	   - https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo?login=test&password=testpass&page=3    - вернёт третью страницу и т.д.
	  
	    - https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo?login=test&password=testpass&page=-1   - если параметр page отрицательный, сервис загрузит все данные, но яндекс вернёт ошибку "JobResponseTooLong"
	  
	   -  https://functions.yandexcloud.net/d4euusq7ilpkhc64hqoo?login=test&password=testpass&page=3000 - если параметр page больше кол-ва страниц, вернётся последняя страница
	  
	  
* На момент разработки регионов было 85. Оптимальный размер страницы при таком количестве - 5 элементов, в этом случае ошибок не возникает
	  
* Размер страницы можно установить в файле appsettings.json проекта "CourtList.Yandex", правильные логин и пароль устанавливаются там же
	  
	  
### Описание проектов:

* CourtList.Services - сервисы для получения регионов, судов; для работы с HTML
* CourtList.Models   - общие модели
* CourtList.Console  - просто консольное приложение, получающее регионы и выводящее их в консоль
* CourtList.Tests    - тесты
* CourtList.Yandex   - проект с функцией-обработчиком для "Yandex Cloud Functions", описанной здесь https://cloud.yandex.ru/docs/functions/lang/csharp/model/independent-function
	  
	  
### Публикация в "Yandex Cloud Functions":
 
* Открыть решение в Visual Studio или Rider
* Если нужно изменить кол-во элементов на странице или другие настройки, поменять их можно в файле appsettings.json проекта "CourtList.Yandex"
* Восстановить nuget'ы, пересобрать проекты, убедиться, что ошибок нет
* Открыть консоль (cmd)
* В консоле перейти в папку с проектом CourtList.Yandex(папка, где csproj файл проекта)
* Выполнить команду dotnet publish
* В папке с проектом перейти в папку \bin\Debug\net6.0\publish
* Выделить все файлы и создать zip архив с ними
* Создать облачную функцию яндекс, подробности здесь -> https://cloud.yandex.ru/docs/functions/concepts/function
* Перейти в панель управления облачной функцией яндекс (https://console.cloud.yandex.ru/folders/b1gbbir4fjhvpgr93641/functions/functions/d4euusq7ilpkhc64hqoo/editor)       
* В левом меню выбрать пункт "Редактор"
* На странице редактора выбрать вкладку "ZIP-архив"
* Прикрепить созданный архив, нажать кнопку "Создать версию", в правом верхнем углу
* Если ошибок не возникло, версия функции будет работать по соответствующему адресу
