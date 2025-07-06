// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "Delegates/DelegateCombinations.h"
#include "HealthComponent.generated.h"


UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class INTERACTIVEUE_API UHealthComponent : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UHealthComponent();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Health")
	float MaxHealth = 100.0f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Health")
	float CurrentHealth = 100.0f;

	UPROPERTY()
	float DamageCooldown = 0.5f; // seconds between hits

	float LastDamageTime = -999.f; // tracks last time damage was taken
	
	UPROPERTY()
	float RedFlashTimer = 0.25f;

public:	
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	UFUNCTION()
	void TakeDamage(float DamageAmount);

	UFUNCTION(BlueprintCallable, Category = "Health")
	float GetHealth() const { return CurrentHealth; }

	UFUNCTION(BlueprintCallable, Category = "Health")
	void AddHealth(float AddedHealth);

	UFUNCTION(BlueprintCallable, Category = "Health")
	bool IsDead() const { return CurrentHealth <= 0.1; }

	UFUNCTION(BlueprintCallable)
	void OnDeath();

	

	FTimerHandle RedFlashTimerHandle;

	UFUNCTION()
	void HideRedFlash();

	UPROPERTY(BlueprintReadOnly)
	bool ShowRed = false;


	DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnHealthChanged);
	UPROPERTY(BlueprintAssignable, Category = "Events")
	FOnHealthChanged OnHealthChanged;
	
	DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnDeathSignal);
	UPROPERTY(BlueprintAssignable, Category = "Events")
	FOnDeathSignal OnDeathEvent;
};
