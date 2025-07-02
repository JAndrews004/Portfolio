// Fill out your copyright notice in the Description page of Project Settings.


#include "FP_Movement/Player/MyCharacter.h"
#include "Camera/CameraComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "GameFramework/SpringArmComponent.h"

// Sets default values
AMyCharacter::AMyCharacter()
{
 	// Set this character to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	SpringArm = CreateDefaultSubobject<USpringArmComponent>(TEXT("Camera Boom"));
	SpringArm->SetupAttachment(RootComponent); // attach to capsule
	SpringArm->bUsePawnControlRotation = true;
	SpringArm->TargetArmLength = 0.f;

	Camera = CreateDefaultSubobject<UCameraComponent>(TEXT("Camera"));
	Camera->SetupAttachment(SpringArm);
	Camera->bUsePawnControlRotation = false;

	
}

// Called when the game starts or when spawned
void AMyCharacter::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void AMyCharacter::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);
	GetCharacterMovement()->GetNavAgentPropertiesRef().bCanCrouch = true;


}

// Called to bind functionality to input
void AMyCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

	PlayerInputComponent->BindAction("Jump", IE_Pressed, this, &ACharacter::Jump);
	PlayerInputComponent->BindAxis("MoveForward", this, &AMyCharacter::MoveForward);
	PlayerInputComponent->BindAxis("MoveRight", this, &AMyCharacter::MoveRight);


	PlayerInputComponent->BindAxis("Turn", this, &AMyCharacter::Turn);
	PlayerInputComponent->BindAxis("LookUp", this, &AMyCharacter::LookUp);

	PlayerInputComponent->BindAction("Sprint", IE_Pressed, this, &AMyCharacter::StartSprint);
	PlayerInputComponent->BindAction("Sprint", IE_Released, this, &AMyCharacter::EndSprint);

	PlayerInputComponent->BindAction("Crouch", IE_Pressed, this, &AMyCharacter::SetCrouch);
	PlayerInputComponent->BindAction("Crouch", IE_Released, this, &AMyCharacter::SetUncrouch);
}

void AMyCharacter::MoveForward(float Input)
{
	FVector ForwardDirection = GetActorForwardVector();
	AddMovementInput(ForwardDirection, Input);
}

void AMyCharacter::MoveRight(float Input)
{
	FVector RightDirection = GetActorRightVector();
	AddMovementInput(RightDirection, Input);
}

void AMyCharacter::Turn(float Input)
{
	AddControllerYawInput(Input);
}

void AMyCharacter::LookUp(float Input)
{
	AddControllerPitchInput(Input);
}

void AMyCharacter::StartSprint()
{
	GetCharacterMovement()->MaxWalkSpeed = SprintSpeed;
}

void AMyCharacter::EndSprint()
{
	GetCharacterMovement()->MaxWalkSpeed = WalkSpeed;
}

void AMyCharacter::SetCrouch()
{
	Crouch();
	GetCharacterMovement()->MaxWalkSpeed = CrouchSpeed;

}

void AMyCharacter::SetUncrouch() {
	UnCrouch();
	GetCharacterMovement()->MaxWalkSpeed = WalkSpeed;
}

