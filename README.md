# PolyCraft
### 누구나 VR 환경에서 쉽게 간단한 3D 모델을 만들 수 있는 프로젝트
‘PolyCraft’는 3D 모델링 경험이 없는 사용자도 VR 환경에서 직관적인 상호작용으로 3D 모델을 만들 수 있는 프로젝트입니다. ‘2024 메타버스 개발자 경진대회’에 참여했습니다.
- 플랫폼 : Windows
- 장르 : VR
- 개발 기간 : 2024.05.07 ~ 2024.08.14
- 개발 인원 : 2인

## Link
- [Notion Page](https://www.notion.so/e529935ed71049e4ba7dbcb2d4bcc70b?pvs=21)
- [Download](https://github.com/teinoi/PolyCraft/raw/main/PolyCraft.zip)
- [Demo Video](https://youtu.be/7f305gugM6w?si=R_BqBzgRVLEvpUb7)

## Tech Stack
- Unity 2023.2.13f1(URP)
- OpenXR
- XR Interaction Toolkit

## 주요 특징
### 유니티 VR 환경 구축
OpenXR 플러그인과 XR Interaction Toolkit을 사용하여 Meta Quest의 사용 환경을 구축

### 컨트롤러 상호작용 설계
- 오브젝트 생성과 같은 기능들을 UI 버튼 클릭으로 실행 가능하도록 구현
- 왼쪽 컨트롤러는 오브젝트 선택과 조작을, 오른쪽 컨트롤러는 UI 상호작용을 담당하도록 설계

<img src="https://github.com/user-attachments/assets/ca57261d-bad8-43e2-8b27-a59bce7fca68" width="400"/>

### 오브젝트 생성 시스템 구현
- 사용자가 선택한 3D 프리팹을 생성하고 컨트롤러와 연동되어 있는 XRRayInteractor의 Ray가 충돌한 지점에 배치할 수 있게 구현
- 생성된 오브젝트에 필수 컴포넌트(Collider, Rigidbody 등)를 자동으로 추가하여 원활한 상호작용이 가능하도록 설계

<img src="https://github.com/user-attachments/assets/14d7339f-60cd-4d5f-8c63-f45c41cd5743" width="400"/>

### 오브젝트 복사/병합/삭제/색상 변경 시스템 구현
- 사용자가 시각적으로 조작 대상을 명확하게 하기위해 [Quick Outline](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488?locale=ko-KR&srsltid=AfmBOopIEworgGMAKM-FB0nw0e2KEV573TOIOh0A2ws91LUwF1e07xzJ) 에셋을 활용하여 선택한 오브젝트의 외곽선(Outline)이 강조되는 시각적 효과를 구현
- 선택한 오브젝트(외곽선이 강조된 오브젝트)를 대상으로 복사, 병합, 삭제, 색상 변경 시스템을 동작 가능하도록 설계

<img src="https://github.com/user-attachments/assets/8ca8fca0-202f-4322-89db-2d56b5ecc4fa" width="400"/>

### 오브젝트 이동/회전/크기 변경 시스템 구현
사용자가 오브젝트를 선택한 후 원하는 축(X/Y/Z)으로 이동/회전/크기 조절이 가능하도록 구현

<img src="https://github.com/user-attachments/assets/0bc6070f-f5c3-4662-9fd9-15d999bca1df" width="400"/>
