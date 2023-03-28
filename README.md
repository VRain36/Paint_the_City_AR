[![Paint_The_City_AR](https://user-images.githubusercontent.com/90603530/222171371-d0dffb0c-b0c3-44bf-a996-36d82961f77d.jpg)](https://github.com/VRain36/Paint_the_City_AR)



## 시작 가이드
### 1️⃣ 모바일로 실행하기 
apk 파일을 다운로드 받아서 실행합니다.          
[![Paint_The_City_AR](https://user-images.githubusercontent.com/90603530/222171835-9c3ca7af-0eb9-406f-b58c-5518de3c90e9.jpg)](https://drive.google.com/file/d/1CaDbDbbD4xFqYHficVosOErtLRjRutDa/view?usp=sharing)


### 2️⃣ PC로 실행하기     
#### Requirement 
- [Unity 2021.3.10f1](https://unity.com/releases/editor/archive)
#### Installation 
```
  $ git clone https://github.com/VRain36/Paint_the_City_AR.git
```
git clone 후, 📁 Assets/Scenes 폴더 아래에 있는 `PaintTheCity_AR.unity` 파일을 실행합니다.   
    


## 주요 기능    
✅ **public/private mode**     
  ```
  로그인 한 유저는 자신이 private 모드를 선택하여 저장한 작품을 확인할 수 있으며, 
  다른 사람들에게는 이 작품이 공개되지 않습니다.
  public 모드를 선택하여 저장한 작품은 모든 사람들에게 공개됩니다.
  
  유저는 화면 상단의 public/private 탭을 눌러서 각 모드별 작품을 확인할 수 있습니다.
  ```
✅ **size scaling**    
```
  관람하기에 작품의 크기가 작거나 큰 경우, 화면 하단의 슬라이더 바를 이용하여, 작품의 크기를 조절할 수 있습니다.
```
✅ **capture**
```
  원하는 작품과 함께 사진 촬영할 수 있습니다.
```
✅ **user's location check**
```
  아래 두 가지의 조건을 모두 만족하면, 사용자가 해당 장소에 도착했다고 판단합니다.
  1) GPS 기능을 기반으로, 사용자의 위치 정보 (위도, 경도)를 받아서 위치를 확인합니다.
     이때, (위도, 경도)의 값은 소수점 두 번째 자리까지 표현할 경우, 범위가 정확하지 않을 수도 있다는 점을 고려하여
  2) 이미지 타겟팅 기능을 기반으로, 사용자의 AR 카메라로 특정 이미지를 인식하는 경우 작품을 띄웁니다.
```

## 화면 구성
☑️ 화면 단위 캡처하여, 설명과 함께 첨부하기 

## 아키텍처 
☑️ 프론트엔드, 백엔드 연결부분 서술하기     
☑️ ERD 추가하여, 데이터 베이스 구현부분 설명하기

## 개발 환경 
|Frontend|Backend/Cloud|
|:------:|:---:|
|Frontend|<img src="https://img.shields.io/badge/Unity-FFFFFF?style=for-the-badge&logo=Unity&logoColor=black">|
|Backend|<img src="https://img.shields.io/badge/mysql-4479A1?style=for-the-badge&logo=mysql&logoColor=white"> <img src="https://img.shields.io/badge/python-3776AB?style=for-the-badge&logo=python&logoColor=white">  <img src="https://img.shields.io/badge/AWS Lambda-FF9900?style=for-the-badge&logo=AWS Lambda&logoColor=black">     <img src="https://img.shields.io/badge/Amazon S3-569A31?style=for-the-badge&logo=Amazon S3&logoColor=black">     <img src="https://img.shields.io/badge/Amazon RDS-FF9900?style=for-the-badge&logo=Amazon RDS&logoColor=black">       <img src="https://img.shields.io/badge/Amazon API Gateway-FF4F8B?style=for-the-badge&logo=Amazon API Gateway&logoColor=black">|
|Collaboration|<img src="https://img.shields.io/badge/Notion-000000?style=for-the-badge&logo=Notion&logoColor=white">   <img src="https://img.shields.io/badge/Zoom-2D8CFF?style=for-the-badge&logo=Zoom&logoColor=white">|
