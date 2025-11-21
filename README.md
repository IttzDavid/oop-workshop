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
