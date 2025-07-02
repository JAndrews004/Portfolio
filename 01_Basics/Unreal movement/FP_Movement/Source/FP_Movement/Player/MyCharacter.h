// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "MyCharacter.generated.h"

UCLASS()
class FP_MOVEMENT_API AMyCharacter : public ACharacter
{
	GENERATED_BODY()

public:
	// Sets default values for this character's properties
	AMyCharacter();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

protected:
	UPROPERTY(EditAnywhere)
	class UCameraComponent* Camera;

	UPROPERTY(EditAnywhere)
	class USpringArmComponent* SpringArm;

	void MoveForward(float Input);
	void MoveRight(float  Input);

	void Turn(float Input);
	void LookUp(float  Input);

	void StartSprint();
	void EndSprint();

	void SetCrouch();
	void SetUncrouch();

	float WalkSpeed = 800.0f;
	float SprintSpeed = 1600.0f;
	float CrouchSpeed = 400.0f;
};
