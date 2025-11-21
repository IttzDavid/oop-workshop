# oop-workshop

In this OOP workshop we were tasked to make a system for Sønderborg’s library. Made by David Takac, Samuel Toman and Adam Kondrcik.

# Functional and Non-Functional Requirements

| Functional Requirement                                       | Description                                                                 |
|--------------------------------------------------------------|-----------------------------------------------------------------------------|
| **1. List media items**                                       | Display a list of items.                                                    |
| **2. Sort media items**                                       | Sort items by a category/logic.                                             |
| **3. Categorize media items**                                 | Items must be sorted into defined categories.                               |
| **4. Allow user to download items**                           | User must be able to download items.                                        |
| **5. Allow user who downloaded an item to rate it**           | User must be able to rate downloaded items.                                 |
| **6. Allow user to borrow media items**                       | User must be able to borrow media items.                                    |
| **7. Support three categories of users**                      | System must support 3 categories of user: Borrower, Employee, Admin.        |
| **8. Allow user to identify themselves**                      | The system must ask user to identify themselves (name, age, and social security number).      |
| **9. Allow management of items and users for select category of user**| Depending upon the type of user, user must be able to manage media items and users. |

| Non-Functional Requirement                                    | Description                                                                 |
|---------------------------------------------------------------|-----------------------------------------------------------------------------|
| **1. Capacity**                                                | The system must allow for storage of hundreds of items.                     |
| **2. Performance**                                             | The system must be performant.                                              |
| **3. Scalability**                                             | The system must support easy addition of new user roles and media types.    |
| **4. Security**                                                | The system must be secure.                                                  |
| **5. Usability**                                               | The system must guide the user with easy to understand instructions and verify inputs. |
| **6. Maintainability**                                         | The system must be easy to maintain.                                        |

# CRC and UML Diagram

| **Class**            | **Admin**                                                                                                                                                            |
| -------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - Manage media items (add, delete, update). <br> - Manage users (add, delete, update). <br> - View users.                                                            |
| **Collaborations**   | - **MediaItem**: Manages media items. <br> - **Employee**: Collaborates to manage media items and users. <br> - **Borrower**: Collaborates to manage borrow records. |

| **Class**            | **Employee**                                                                                                                                                                      |
| -------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - Add media items. <br> - View users. <br> - View media items.                                                                                                                    |
| **Collaborations**   | - **MediaItem**: Adds and views media items. <br> - **Admin**: Collaborates with Admin to manage media items and users. <br> - **Borrower**: Interacts with borrower information. |

| **Class**            | **Borrower**                                                                                                                                                                                                         |
| -------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - View media items. <br> - Rate media items. <br> - Borrow media items.                                                                                                                                              |
| **Collaborations**   | - **MediaItem**: Views and rates media items. <br> - **Admin**: Views information about admins and employees for potential issues. <br> - **Employee**: Can interact with media items or get support from employees. |

| **Class**            | **MediaItem**                                                                                                                      |
| -------------------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - Store media item details (title, genre, release year). <br> - Allow users to view details. <br> - Allow downloading of the item. |
| **Collaborations**   | - **Ebook, Movie, Song, VideoGame, App, Podcast, Image**: Inherited by these specific types of media items.                        |

| **Class**            | **Ebook**                                                                                                                                                                       |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - Store Ebook-specific details like author, pages, ISBN. <br> - Allow user to view and download the Ebook.                                                                      |
| **Collaborations**   | - **MediaItem**: Inherits general media item behavior. <br> - **Borrower**: Interacts with Ebook by downloading and rating. <br> - **Employee**: Adds or updates Ebook records. |

| **Class**            | **Movie**                                                                                                                                              |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Responsibilities** | - Store Movie-specific details like director, genre, duration. <br> - Allow users to view and download the movie.                                      |
| **Collaborations**   | - **MediaItem**: Inherits general media item behavior. <br> - **Borrower**: Views and rates the movie. <br> - **Employee**: Manages the movie records. |

| **Class**            | **Song**                                                                                                                                                                        |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - Store Song-specific details like composer, singer, genre. <br> - Allow users to view and download the song.                                                                   |
| **Collaborations**   | - **MediaItem**: Inherits general media item behavior. <br> - **Borrower**: Interacts with the song by listening and rating. <br> - **Employee**: Adds or updates song records. |

| **Class**            | **VideoGame**                                                                                                                                                        |
| -------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - Store VideoGame-specific details like publisher, platform, release year. <br> - Allow users to view and download the game.                                         |
| **Collaborations**   | - **MediaItem**: Inherits general media item behavior. <br> - **Borrower**: Views and rates the video game. <br> - **Employee**: Adds or updates video game records. |

| **Class**            | **App**                                                                                                                                                 |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - Store App-specific details like version, platform, file size. <br> - Allow users to view and download the app.                                        |
| **Collaborations**   | - **MediaItem**: Inherits general media item behavior. <br> - **Borrower**: Downloads and rates apps. <br> - **Employee**: Adds or updates app records. |

| **Class**            | **Podcast**                                                                                                                                                 |
| -------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - Store Podcast-specific details like host, guest, episode number. <br> - Allow users to view and listen to the podcast.                                    |
| **Collaborations**   | - **MediaItem**: Inherits general media item behavior. <br> - **Borrower**: Views and rates podcasts. <br> - **Employee**: Adds or updates podcast records. |

| **Class**            | **Image**                                                                                                                                                  |
| -------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Responsibilities** | - Store Image-specific details like resolution, file format, and date taken. <br> - Allow users to view and download the image.                            |
| **Collaborations**   | - **MediaItem**: Inherits general media item behavior. <br> - **Borrower**: Views and rates the image. <br> - **Employee**: Adds or updates image records. |

```mermaid
classDiagram
    class Admin {
        +String name
        +String age
        +String ssn
        +addMediaItem()
        +manageEmployees()
        +viewUsers()
    }

    class Employee {
        +String name
        +String age
        +String ssn
        +addMediaItem()
        +viewUsers()
    }

    class Borrower {
        +String name
        +String age
        +String ssn
        +viewMediaItems()
        +rateItem()
    }

    class MediaItem {
        +String title
        +String genre
        +String releaseYear
        +download()
        +viewDetails()
    }

    class Ebook {
        +String author
        +String language
        +int pages
        +String ISBN
    }

    class Movie {
        +String director
        +String genre
        +String language
        +int duration
    }

    class Song {
        +String composer
        +String singer
        +String genre
        +String language
        +int duration
    }

    class VideoGame {
        +String publisher
        +String platform
        +int releaseYear
    }

    class App {
        +String version
        +String platform
        +String fileSize
    }

    class Podcast {
        +String host
        +String guest
        +int episodeNumber
        +String language
    }

    class Image {
        +String resolution
        +String fileFormat
        +String dateTaken
    }

    Admin --|> Employee : manages >
    Admin --|> Borrower : manages >
    Admin --> MediaItem : manage >
    Employee --> MediaItem : add/view >
    Borrower --> MediaItem : view/rate >

    MediaItem <|-- Ebook : inherits >
    MediaItem <|-- Movie : inherits >
    MediaItem <|-- Song : inherits >
    MediaItem <|-- VideoGame : inherits >
    MediaItem <|-- App : inherits >
    MediaItem <|-- Podcast : inherits >
    MediaItem <|-- Image : inherits >
