#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "InputActionValue.h"

#include "MyCharacter.generated.h"


class UInputMappingContext;
class UInputAction;


UCLASS()
class FP_MOVEMENT_EI_API AMyCharacter : public ACharacter
{
	GENERATED_BODY()

public:
	// Sets default values for this character's properties
	AMyCharacter();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;


	UPROPERTY(EditAnywhere)
	class UCameraComponent* Camera;

	UPROPERTY(EditAnywhere)
	class USpringArmComponent* SpringArm;

	void Move(const FInputActionValue& Value);
	void Look(const FInputActionValue& Value);
	void Sprint(const FInputActionValue& Value);
	void StopSprint(const FInputActionValue& Value);
	void JumpInput(const FInputActionValue& Value);
	void JumpInputReleased(const FInputActionValue& Value);
	void CrouchInputReleased(const FInputActionValue& Value);
	void CrouchInputPressed(const FInputActionValue& Value);

	UPROPERTY(EditDefaultsOnly, Category = "Enhanced Input")
	UInputMappingContext* PlayerMappingContext;

	UPROPERTY(EditDefaultsOnly, Category = "Enhanced Input")
	UInputAction* MoveAction;

	UPROPERTY(EditDefaultsOnly, Category = "Enhanced Input")
	UInputAction* LookAction;

	UPROPERTY(EditDefaultsOnly, Category = "Enhanced Input")
	UInputAction* SprintAction;

	UPROPERTY(EditDefaultsOnly, Category = "Enhanced Input")
	UInputAction* JumpAction;

	UPROPERTY(EditDefaultsOnly, Category = "Enhanced Input")
	UInputAction* CrouchAction;



public:
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input


};
